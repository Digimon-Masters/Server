using Blazored.LocalStorage;
using DigitalWorldOnline.Admin.Data;
using DigitalWorldOnline.Application.Admin.Repositories;
using DigitalWorldOnline.Application.Extensions;
using DigitalWorldOnline.Commons.Enums;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Repositories.Admin;
using DigitalWorldOnline.Commons.ViewModel.Login;
using DigitalWorldOnline.Infraestructure;
using DigitalWorldOnline.Infraestructure.Extensions;
using DigitalWorldOnline.Infraestructure.Mapping;
using DigitalWorldOnline.Infraestructure.Repositories.Account;
using DigitalWorldOnline.Infraestructure.Repositories.Admin;
using DigitalWorldOnline.Infraestructure.Repositories.Character;
using DigitalWorldOnline.Infraestructure.Repositories.Config;
using DigitalWorldOnline.Infraestructure.Repositories.Routine;
using DigitalWorldOnline.Infraestructure.Repositories.Server;
using MediatR;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MudBlazor.Services;
using Serilog;
using Serilog.Formatting.Compact;
using System;
using System.Configuration;
using System.Reflection;
using System.Security.Claims;

namespace DigitalWorldOnline.Admin
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddBlazoredLocalStorage();
            services.AddMudServices();
            services.AddControllersWithViews();
            services.AddHttpContextAccessor();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Default", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, UserAccessLevelEnum.Default.ToString());
                });

                options.AddPolicy("GameMaster", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, UserAccessLevelEnum.GameMaster.ToString());
                });

                options.AddPolicy("Administrator", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Role, UserAccessLevelEnum.Administrator.ToString());
                });
            });

            services.AddSingleton<LoginModelRepository>();

            services.AddAuthentication(
            options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddCookie(options =>
            {
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.LoginPath = new PathString("/login");
                options.LogoutPath = new PathString("/Logout");
                options.AccessDeniedPath = options.LoginPath;
                options.ReturnUrlParameter = "/";
            });

            services.AddDbContext<DatabaseContext>(options => options.UseSqlServer(Configuration["Database:Connection"]));
            services.AddScoped<IAdminQueriesRepository, AdminQueriesRepository>();
            services.AddScoped<IAdminCommandsRepository, AdminCommandsRepository>();
            
            services.AddScoped<IServerQueriesRepository, ServerQueriesRepository>();
            services.AddScoped<IServerCommandsRepository, ServerCommandsRepository>();

            services.AddScoped<IAccountQueriesRepository, AccountQueriesRepository>();
            services.AddScoped<IAccountCommandsRepository, AccountCommandsRepository>();

            services.AddScoped<ICharacterQueriesRepository, CharacterQueriesRepository>();
            services.AddScoped<ICharacterCommandsRepository, CharacterCommandsRepository>();

            services.AddScoped<IConfigQueriesRepository, ConfigQueriesRepository>();
            services.AddScoped<IConfigCommandsRepository, ConfigCommandsRepository>();

            services.AddScoped<IRoutineRepository, RoutineRepository>();

            services.AddTransient<Mediator>();
            services.AddSingleton<ISender, ScopedSender<Mediator>>();
            services.AddMediatR(typeof(MediatorApplicationHandlerExtension).GetTypeInfo().Assembly);
            services.AddSingleton(ConfigureLogger(Configuration));

            services.AddAutoMapper(typeof(AdminProfile));
            services.AddAutoMapper(typeof(AdminMappingProfile));
            services.AddAutoMapper(typeof(ConfigProfile));
        }

        static ILogger ConfigureLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.RollingFile(configuration["Log:DebugRepository"] ?? "logs\\Debug\\Admin", Serilog.Events.LogEventLevel.Debug, retainedFileCountLimit: 100)
                .WriteTo.RollingFile(configuration["Log:InformationRepository"] ?? "logs\\Information\\Admin", Serilog.Events.LogEventLevel.Information, retainedFileCountLimit: 100)
                .WriteTo.RollingFile(configuration["Log:WarningRepository"] ?? "logs\\Warning\\Admin", Serilog.Events.LogEventLevel.Warning, retainedFileCountLimit: 100)
                .WriteTo.RollingFile(configuration["Log:ErrorRepository"] ?? "logs\\Error\\Admin", Serilog.Events.LogEventLevel.Error, retainedFileCountLimit: 100)
                .CreateLogger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
