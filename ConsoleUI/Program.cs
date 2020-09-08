using ConsoleUI.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.ComponentModel;
using System.IO;

// DI, Serilog, Settings

namespace ConsoleUI
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Dependency injection, Configuration, Loging(SeriLog) in Core Console Application");

            var configurationBuilder = new ConfigurationBuilder();
            BuildConfig(configurationBuilder);

            // Console Logger
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(configurationBuilder.Build())
                .Enrich.FromLogContext()
                .WriteTo.File("log.txt", rollOnFileSizeLimit: true)
                .CreateLogger();

            Log.Logger.Information("Application Starting");

            var host = Host.CreateDefaultBuilder()
                .ConfigureServices((context, services) =>
                {
                    services.AddTransient<IGreetingService, GreetingService>();
                    services.AddTransient<ICalc, CalcService>();
                    services.AddTransient<IHelper, HelperService>();
                })
                .UseSerilog()
                .Build();

            IServiceProvider provider = host.Services;

            // Explicitly create an instance 
            var greetingService = ActivatorUtilities.CreateInstance<GreetingService>(provider);
            greetingService.Run();

            // Implicit way of careating an instance
            var calService  = provider.GetRequiredService<ICalc>();
            calService.Add();

            // Implicit way of careating an instance
            var helerSerice = provider.GetRequiredService<IHelper>();
            helerSerice.Convert();
        }

        static void BuildConfig(IConfigurationBuilder builder)
        {
            builder.SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Production"}.json", optional: true)
                .AddEnvironmentVariables();
        }
    }
}
