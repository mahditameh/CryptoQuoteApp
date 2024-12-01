using Applications.Contracts;
using Applications.Services;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureConfig
{
    public static class ApplicationDependencyInjection
    {
        public static void RegisterServices(this IServiceCollection service, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            service.AddTransient<ICryptoService, CryptoService>();
        }
    }
}
