using ConsoleUIServiceCollectionCoreApp.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceCollectionCoreApp.Core;
using System;

namespace ServiceCollectionCoreApp
{
    class Program
    {
        private static IConfigurationRoot _configuration;

        static void Main(string[] args)
        {
            Console.WriteLine("Dependency injection, Configuration, Loging(SeriLog) in Core Console Application");
            IServiceCollection services = new ServiceCollection();

            ConfigureServices(services);

            IServiceProvider provider = services.BuildServiceProvider();

            var greetingService = provider.GetRequiredService<IGreetingService>();
            //greetingService.LoadConfig(_configuration);
            greetingService.Run();

            var calService = provider.GetService<ICalc>();
            calService.Add();


        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environmentName}.json", true)
                // Override config by env, using like Logging:Level or Logging__Level
                .AddEnvironmentVariables();

            _configuration = builder.Build();


            /*Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(Serilog.Events.LogEventLevel.Debug)
                .CreateLogger();
            */
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Build())
                .Enrich.FromLogContext()
                .WriteTo.File("log.txt", rollOnFileSizeLimit: true)
                .CreateLogger();

            Log.Logger.Information("Starting Soft Quote Service with {Environment} settings file.", environmentName);

            services.AddLogging(configure =>
            {
                configure.SetMinimumLevel(LogLevel.Debug);
                configure.AddSerilog();
            });

            services.AddSingleton(_configuration);
            services.AddScoped<IConfiguration>(_ => _configuration);    


            services.AddTransient<IGreetingService, GreetingService>();
            services.AddTransient<IHelper, HelperService>();
            services.AddTransient<ICalc, CalcService>();

            // services.AddAutoMapper(Assembly.GetExecutingAssembly());
            //services.AddSingleton(provider => new MapperConfiguration(cfg =>
            //{
            //    cfg.AddProfile(new MappingProfile());
            //}).CreateMapper());

        }
    }
}
