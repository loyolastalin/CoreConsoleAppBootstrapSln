using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceCollectionCoreApp.Core;
using System.Linq;

// DI, Serilog, Settings

namespace ServiceCollectionCoreApp
{
    public class GreetingService : IGreetingService
    {
        private readonly ILogger<GreetingService> _log;
        private IConfiguration _config;

        public GreetingService(ILogger<GreetingService> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }
        public GreetingService(ILogger<GreetingService> log)
        {
            _log = log;
           // _config = config;
        }

        public void LoadConfig(IConfiguration config)
        {
            var serilogConfiguration = config.GetSection("Serilog").GetChildren();
            var serilogConfigurationKey = (from c in serilogConfiguration select c.Key).ToList();

            _config = config;
        }

        public void Run()
        {
            for (int i = 0; i < _config.GetValue<int>("LoopTimes"); i++)
            {
                _log.LogError("Run number {runNumber}", i);
            }
        }
    }
}
