﻿// DI, Serilog, Settings

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceCollectionCoreApp.Core;
using System;

namespace ServiceCollectionCoreApp
{
    public class HelperService : IHelper
    {
        private readonly IServiceProvider _provider;

        public HelperService(IServiceProvider provider)
        {
            this._provider = provider;
        }

        public void Convert()
        {
            var config  = _provider.GetRequiredService<IConfiguration>();
            var value  = config.GetValue<int>("LoopTimes");

        }
    }
}
