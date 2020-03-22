using System;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using DotNetOwinWebApiSample.Api;
using DotNetOwinWebApiSample.Api.ExceptionHandler;
using DotNetOwinWebApiSample.Api.Extensions;
using DotNetOwinWebApiSample.Api.Middlewares;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;
using DotNetOwinWebApiSample.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartup(typeof(Startup))]

namespace DotNetOwinWebApiSample.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            // ErrorHandlingMiddlewareで例外処理を行うため、既定の例外制御を抑止します
            configuration.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());

            SwaggerConfig.Register(configuration);

            app.Use<ErrorHandlingMiddleware>();

            var services = new ServiceCollection();
            ConfigureServices(services);

            var provider = services.BuildServiceProvider();
            configuration.Services.Replace(typeof(IHttpControllerActivator), new ServiceProviderControllerActivator(provider));

            app.UseWebApi(configuration);
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddTransient<TodoService>();
            services.AddTransient<GreetingService>();
            services.AddTransient<IRepository<Todo>, TodoRepository>();
            services.AddControllersAsServices();
        }
    }

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