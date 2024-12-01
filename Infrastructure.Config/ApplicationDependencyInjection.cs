using Application;
using Application.Contracts;
using Application.Handlers;
using Infrastructure.Services;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace InfrastructureConfig
{
    public static class ApplicationDependencyInjection
    {
        public static void RegisterServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            services.AddTransient<ICryptoService, CryptoService>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(GetCryptoQuoteQueryHandler).GetTypeInfo().Assembly));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
        }
    }
}
