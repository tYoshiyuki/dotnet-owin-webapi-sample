using System.Reflection;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;
using DotNetOwinWebApiSample.Api;
using DotNetOwinWebApiSample.Api.ExceptionHandler;
using DotNetOwinWebApiSample.Api.Middlewares;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;
using DotNetOwinWebApiSample.Api.Services;
using Microsoft.Owin;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
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
            app.UseNinjectMiddleware(CreateKernel);
            app.UseNinjectWebApi(configuration);
        }

        /// <summary>
        /// DIの設定を行います
        /// </summary>
        /// <returns>StandardKernel</returns>
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