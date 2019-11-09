using System.Collections.Generic;
using System.Reflection;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using DotNetOwinWebApiSample.Api.Controllers;
using DotNetOwinWebApiSample.Api.Middlewares;
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

            // テスト対象となるコントローラを MVC の処理対象に登録します
            configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
            configuration.MapHttpAttributeRoutes();

            app.Use<ErrorHandlingMiddleware>();
            TestUtil.Kernel = CreateKernel();
            app.UseNinjectMiddleware(() => TestUtil.Kernel);
            app.UseNinjectWebApi(configuration);
        }

        /// <summary>
        /// DIの設定を行います
        /// </summary>
        /// <returns></returns>
        private static IKernel CreateKernel()
        {
            var kernel = new StandardKernel();
            kernel.Bind<TodoService>().ToSelf().InSingletonScope();
            kernel.Bind<IRepository<Todo>>().To<TodoRepository>().InSingletonScope();
            kernel.Load(Assembly.GetExecutingAssembly());
            return kernel;
        }

        /// <summary>
        /// テスト対象となるコントローラのアセンブリを取得します
        /// </summary>
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
        /// <summary>
        /// テスト用にDIコンテナへのアクセッサを提供します
        /// </summary>
        public static IKernel Kernel { get; set; }
    }
}
