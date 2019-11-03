﻿using System.Reflection;
using System.Web.Http;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;
using DotNetOwinWebApiSample.Api.Services;
using Microsoft.Owin;
using Ninject;
using Owin;
using Ninject.Web.Common.OwinHost;
using Ninject.Web.WebApi.OwinHost;

[assembly: OwinStartup(typeof(DotNetOwinWebApiSample.Api.Startup))]

namespace DotNetOwinWebApiSample.Api
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.MapHttpAttributeRoutes();

            SwaggerConfig.Register(configuration);

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
