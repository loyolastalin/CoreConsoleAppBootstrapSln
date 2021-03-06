﻿using AutoMapper;
using ConsoleUIServiceCollectionCoreApp.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using ServiceCollectionCoreApp.Config;
using ServiceCollectionCoreApp.Core;
using ServiceCollectionCoreApp.Model;
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

            var mapper = provider.GetService<IMapper>();

           var user =  mapper.Map<User>(new UserDto() { Name ="sdfsdf", Age =21, Sex ="F" });

        }

        private static void ConfigureServices(IServiceCollection services)
        {
            var environmentName = Environment.GetEnvironmentVariable("CORE_CONSOLE_ENV");

            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings_{environmentName}.json", true)
                // Override config by env, using like Logging:Level or Logging__Level
                .AddEnvironmentVariables();

            _configuration = builder.Build();

            // Reading the values from appsettings, appsetting.dev, environment variable respectiviltily
            var appsetting = _configuration.GetValue<int>("LoopTimes");
            var appsetting_env = _configuration.GetValue<string>("connectionstring");
            var enReading = _configuration.GetValue<string>("Test1");

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

            // Auto Mapper Configurations
            services.AddSingleton(new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            }).CreateMapper());

        }
    }
}
