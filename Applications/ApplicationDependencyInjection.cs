using Applications.Contracts;
using Applications.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Applications
{
    public static class ApplicationDependencyInjection
    {
        public static void RegisterServices(this IServiceCollection service, IConfiguration configuration)
        {
            service.AddTransient<ICryptoService, CryptoService>();
        }
    }
}
