using Microsoft.Extensions.Configuration;

namespace ServiceCollectionCoreApp.Core
{
    public interface IGreetingService
    {
        void LoadConfig(IConfiguration config);
        void Run();
    }
}