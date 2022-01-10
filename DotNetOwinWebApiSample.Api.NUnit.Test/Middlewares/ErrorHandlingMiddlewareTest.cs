using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using DotNetOwinWebApiSample.Api.ExceptionHandler;
using DotNetOwinWebApiSample.Api.Middlewares;
using Microsoft.Owin.Testing;
using Newtonsoft.Json;
using NUnit.Framework;
using Owin;

namespace DotNetOwinWebApiSample.Api.NUnit.Test.Middlewares
{
    public class ErrorHandlingMiddlewareTest
    {
        private readonly string _url = "http://localhost/api/test";
        private static TestServer Server { get; set; }
        private static HttpClient Client { get; set; }

        [OneTimeSetUp]
        public static void Setup()
        {
            Server = TestServer.Create<TestStartup>();
            Client = new HttpClient(Server.Handler);
        }

        [Test]
        public async Task HandleException()
        {
            // Act
            var response = await Client.GetAsync(_url);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            // Assert
            Assert.AreEqual("This is error message.", result["error"]);
        }

        [OneTimeTearDown]
        public static void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }

    [RoutePrefix("api/test")]
    public class TestController : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            throw new Exception("This is error message.");
        }
    }

    public class TestStartup
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolver());
            configuration.MapHttpAttributeRoutes();

            configuration.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());
            app.Use<ErrorHandlingMiddleware>();
            app.UseWebApi(configuration);
        }

        public class TestWebApiResolver : DefaultAssembliesResolver
        {
            public override ICollection<Assembly> GetAssemblies()
            {
                return new List<Assembly> { typeof(TestController).Assembly };
            }
        }
    }
}
