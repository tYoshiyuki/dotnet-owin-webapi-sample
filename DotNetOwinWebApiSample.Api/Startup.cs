using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using DotNetOwinWebApiSample.Api.Middlewares;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;
using DotNetOwinWebApiSample.Api.Services;
using Microsoft.Owin;
using Ninject;
using Owin;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using DotNetOwinWebApiSample.Api.ExceptionHandler;

[assembly: OwinStartup(typeof(DotNetOwinWebApiSample.Api.Startup))]

namespace DotNetOwinWebApiSample.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();
            // ErrorHandlingMiddlewareで例外処理を統括するため、既定の例外制御を抑止する
            configuration.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());

            SwaggerConfig.Register(configuration);

            app.Use<ErrorHandlingMiddleware>();
            app.UseNinjectMiddleware(CreateKernel);
            app.UseNinjectWebApi(configuration);
        }

        private static StandardKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TodoService>().ToSelf().InSingletonScope();
            kernel.Bind<IRepository<Todo>>().To<TodoRepository>().InSingletonScope();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }
    }
}
