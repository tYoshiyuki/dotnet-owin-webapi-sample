using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using DotNetOwinWebApiSample.Api.Controllers;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;
using DotNetOwinWebApiSample.Api.Services;
using Ninject;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;
using Owin;

namespace DoteNetOwinWebApiSample.Api.Test
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
            configuration.MapHttpAttributeRoutes();

            TestUtil.Kernel = CreateKernel();
            app.UseNinjectMiddleware(() => TestUtil.Kernel);
            app.UseNinjectWebApi(configuration);
        }

        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TodoService>().ToSelf().InSingletonScope();
            kernel.Bind<IRepository<Todo>>().To<TodoRepository>().InSingletonScope();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        public class TestWebApiResolver : DefaultAssembliesResolver
        {
            public override ICollection<Assembly> GetAssemblies()
            {
                return new List<Assembly>{ typeof(TodoController).Assembly };
            }
        }

    }

    public class TestUtil
    {
        public static IKernel Kernel { get; set; }
    }
}
