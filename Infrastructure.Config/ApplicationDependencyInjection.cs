using Application;
using Application.Handlers;
using Domain.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfrastructureConfig
{
    public static class ApplicationDependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddScoped<CryptoDomainService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCryptoQuoteQueryHandler).GetTypeInfo().Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
