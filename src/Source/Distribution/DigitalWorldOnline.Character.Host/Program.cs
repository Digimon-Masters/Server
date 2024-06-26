﻿using DigitalWorldOnline.Application.Admin.Repositories;
using DigitalWorldOnline.Application.Extensions;
using DigitalWorldOnline.Application.Services;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Repositories.Admin;
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
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using System.Globalization;
using System.Reflection;
using DigitalWorldOnline.Commons.Utils;
using DigitalWorldOnline.Infraestructure.Repositories.Shop;

namespace DigitalWorldOnline.Character
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Run();
        }

        private static void UnhandledExceptionHandler(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.IsTerminating)
                Console.WriteLine("Terminating by unhandled exception...");
            else
                Console.WriteLine("Received unhandled exception.");

            Console.ReadLine();
        }

        public static IHost CreateHostBuilder(string[] args)
        {
            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionHandler;
            Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;

            return Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .UseEnvironment("Development")
                .ConfigureServices((context, services) =>
                {
                    services.AddDbContext<DatabaseContext>();

                    services.AddScoped<IAdminQueriesRepository, AdminQueriesRepository>();
                    services.AddScoped<IAdminCommandsRepository, AdminCommandsRepository>();

                    services.AddScoped<IAccountQueriesRepository, AccountQueriesRepository>();
                    services.AddScoped<IAccountCommandsRepository, AccountCommandsRepository>();

                    services.AddScoped<IServerQueriesRepository, ServerQueriesRepository>();
                    services.AddScoped<IServerCommandsRepository, ServerCommandsRepository>();

                    services.AddScoped<ICharacterQueriesRepository, CharacterQueriesRepository>();
                    services.AddScoped<ICharacterCommandsRepository, CharacterCommandsRepository>();

                    services.AddScoped<IConfigQueriesRepository, ConfigQueriesRepository>();
                    services.AddScoped<IConfigCommandsRepository, ConfigCommandsRepository>();

                    services.AddScoped<IRoutineRepository, RoutineRepository>();
                    services.AddScoped<ICashShopRepository, CashShopRepository>();

                    //services.AddScoped<IEmailService, EmailService>();

                    services.AddSingleton<ISender, ScopedSender<Mediator>>();
                    services.AddSingleton<IProcessor, CharacterPacketProcessor>();
                    services.AddSingleton(ConfigureLogger(context.Configuration));

                    services.AddHostedService<CharacterServer>();
                    services.AddMediatR(typeof(MediatorApplicationHandlerExtension).GetTypeInfo().Assembly);
                    services.AddTransient<Mediator>();

                    services.AddAutoMapper(typeof(AccountProfile));
                    services.AddAutoMapper(typeof(AssetsProfile));
                    services.AddAutoMapper(typeof(CharacterProfile));
                    services.AddAutoMapper(typeof(ConfigProfile));
                    services.AddAutoMapper(typeof(DigimonProfile));
                    services.AddAutoMapper(typeof(GameProfile));
                    services.AddAutoMapper(typeof(SecurityProfile));
                    services.AddAutoMapper(typeof(ArenaProfile));
                })
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory())
                              .AddEnvironmentVariables(Constants.Configuration.EnvironmentPrefix)
                              .AddUserSecrets<Program>();
                })
                .Build();
        }

        private static ILogger ConfigureLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                    .WriteTo.RollingFile(configuration["Log:VerboseRepository"] ?? "logs\\Verbose\\CharacterServer", retainedFileCountLimit: 10))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                    .WriteTo.RollingFile(configuration["Log:DebugRepository"] ?? "logs\\Debug\\CharacterServer", retainedFileCountLimit: 5))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.RollingFile(configuration["Log:InformationRepository"] ?? "logs\\Information\\CharacterServer", retainedFileCountLimit: 5))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                    .WriteTo.RollingFile(configuration["Log:WarningRepository"] ?? "logs\\Warning\\CharacterServer", retainedFileCountLimit: 5))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.RollingFile(configuration["Log:ErrorRepository"] ?? "logs\\Error\\CharacterServer", retainedFileCountLimit: 5))
                .CreateLogger();
        }
    }
}