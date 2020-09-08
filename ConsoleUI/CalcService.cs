using ConsoleUI.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

// DI, Serilog, Settings

namespace ConsoleUI
{
    public class CalcService : ICalc
    {
        private readonly ILogger<CalcService> _log;
        private readonly IConfiguration _config;

        public CalcService(ILogger<CalcService> log, IConfiguration config)
        {
            this._log = log;
            this._config = config;
        }
        public int Add()
        {
            return 1;
        }
    }
}
