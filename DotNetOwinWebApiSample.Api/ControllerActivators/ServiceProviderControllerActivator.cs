using System;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using Microsoft.Extensions.DependencyInjection;

namespace DotNetOwinWebApiSample.Api.ControllerActivators
{
    public class ServiceProviderControllerActivator : IHttpControllerActivator
    {
        private readonly IServiceProvider _provider;

        public ServiceProviderControllerActivator(IServiceProvider provider)
        {
            _provider = provider;
        }

        public IHttpController Create(HttpRequestMessage request, HttpControllerDescriptor controllerDescriptor, Type controllerType)
        {
            var scopeFactory = _provider.GetRequiredService<IServiceScopeFactory>();
            var scope = scopeFactory.CreateScope();
            request.RegisterForDispose(scope);

            return scope.ServiceProvider.GetService(controllerType) as IHttpController;
        }
    }
}