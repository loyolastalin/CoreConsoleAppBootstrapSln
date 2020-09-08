using ConsoleUIServiceCollectionCoreApp.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ServiceCollectionCoreApp.Core;

// DI, Serilog, Settings

namespace ServiceCollectionCoreApp
{
    public class CalcService : ICalc
    {
        private readonly ILogger<CalcService> _log;
        private readonly IConfiguration _config;
        private readonly IHelper _helper;

        public CalcService(ILogger<CalcService> log, IConfiguration config, IHelper helper)
        {
            this._log = log;
            this._config = config;
            this._helper = helper;
        }
        public int Add()
        {
            return 1;
        }
    }
}
