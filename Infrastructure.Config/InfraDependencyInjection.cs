using Domain;
using Infrastructure;
using Infrastructure.Configurations;
using Infrastructure.Validators;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InfrastructureConfig
{
    public static class InfraDependencyInjection
    {
        public static void RegisterRepositories(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<ICryptoRepository, CryptoRepository>();
            services.Configure<ThirdPartySettings>(configuration.GetSection("ThirdPartySettings"));
            services.AddTransient<ICryptoValidator, CryptoValidator>();
        }
    }
}
