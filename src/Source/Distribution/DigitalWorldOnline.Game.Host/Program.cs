using DigitalWorldOnline.Application;
using DigitalWorldOnline.Application.Admin.Repositories;
using DigitalWorldOnline.Application.Extensions;
using DigitalWorldOnline.Application.Services;
using DigitalWorldOnline.Commons.Interfaces;
using DigitalWorldOnline.Commons.Repositories.Admin;
using DigitalWorldOnline.Game.Managers;
using DigitalWorldOnline.GameHost;
using DigitalWorldOnline.GameHost.EventsServer;
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

namespace DigitalWorldOnline.Game
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

                    //services.AddScoped<IEmailService, EmailService>();

                    services.AddSingleton<DropManager>();
                    services.AddSingleton<StatusManager>();
                    services.AddSingleton<ExpManager>();
                    services.AddSingleton<PartyManager>();

                    services.AddSingleton<EventQueueManager>();
                    
                    services.AddSingleton<MapServer>();
                    services.AddSingleton<PvpServer>();
                    //services.AddSingleton<EventServer>();
                    services.AddSingleton<DungeonsServer>();
                    services.AddSingleton<AssetsLoader>();
                    services.AddSingleton<ConfigsLoader>();
                    services.AddSingleton<GameMasterCommandsProcessor>();

                    services.AddSingleton<ISender, ScopedSender<Mediator>>();
                    services.AddSingleton<IProcessor, GamePacketProcessor>();
                    services.AddSingleton(ConfigureLogger(context.Configuration));

                    services.AddHostedService<GameServer>();

                    services.AddMediatR(typeof(MediatorApplicationHandlerExtension).GetTypeInfo().Assembly);
                    services.AddTransient<Mediator>();

                    AddAutoMapper(services);
                    AddProcessors(services);
                })
                .ConfigureHostConfiguration(hostConfig =>
                {
                    hostConfig.SetBasePath(Directory.GetCurrentDirectory());
                    hostConfig.AddEnvironmentVariables("DSO_");
                })
                .Build();
        }

        private static void AddAutoMapper(IServiceCollection services)
        {
            services.AddAutoMapper(typeof(AccountProfile));
            services.AddAutoMapper(typeof(AssetsProfile));
            services.AddAutoMapper(typeof(CharacterProfile));
            services.AddAutoMapper(typeof(ConfigProfile));
            services.AddAutoMapper(typeof(DigimonProfile));
            services.AddAutoMapper(typeof(GameProfile));
            services.AddAutoMapper(typeof(SecurityProfile));
            services.AddAutoMapper(typeof(ArenaProfile));
        }

        private static void AddProcessors(IServiceCollection services)
        {
            var packetProcessors = Assembly.GetExecutingAssembly()
                .GetTypes()
                .Where(t => typeof(IGamePacketProcessor).IsAssignableFrom(t) && !t.IsInterface)
                .ToList();

            packetProcessors.ForEach(processor =>
            {
                services.AddSingleton(typeof(IGamePacketProcessor), processor);
            });
        }

        private static ILogger ConfigureLogger(IConfiguration configuration)
        {
            return new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {Message:lj}{NewLine}{Exception}", restrictedToMinimumLevel: LogEventLevel.Information)
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Verbose)
                    .WriteTo.RollingFile(configuration["Log:VerboseRepository"] ?? "logs\\Verbose\\GameServer", retainedFileCountLimit: 10))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Debug)
                    .WriteTo.RollingFile(configuration["Log:DebugRepository"] ?? "logs\\Debug\\GameServer", retainedFileCountLimit: 5))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
                    .WriteTo.RollingFile(configuration["Log:InformationRepository"] ?? "logs\\Information\\GameServer", retainedFileCountLimit: 5))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
                    .WriteTo.RollingFile(configuration["Log:WarningRepository"] ?? "logs\\Warning\\GameServer", retainedFileCountLimit: 5))
                .WriteTo.Logger(lc => lc
                    .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
                    .WriteTo.RollingFile(configuration["Log:ErrorRepository"] ?? "logs\\Error\\GameServer", retainedFileCountLimit: 5))
                .CreateLogger();
        }
    }
}