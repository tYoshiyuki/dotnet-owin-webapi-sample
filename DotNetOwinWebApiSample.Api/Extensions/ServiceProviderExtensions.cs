using System;
using System.Linq;
using System.Web.Http.Controllers;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetOwinWebApiSample.Api.Extensions
{
    public static class ServiceProviderExtensions
    {
        public static IServiceCollection AddControllersAsServices(this IServiceCollection services)
        {
            typeof(Startup).Assembly.GetExportedTypes()
                .Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition)
                .Where(t => typeof(IHttpController).IsAssignableFrom(t) || t.Name.EndsWith("Controller", StringComparison.OrdinalIgnoreCase))
                .ToList()
                .ForEach(_ => services.AddScoped(_));

            return services;
        }
    }
}