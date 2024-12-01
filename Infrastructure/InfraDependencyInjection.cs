using Domins;
using Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class InfraDependencyInjection
    {
        public static void RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICryptoRepository, CryptoRepository>();
            services.Configure<ThirdPartySettings>(configuration.GetSection("ThirdPartySettings"));

        }
    }
}
