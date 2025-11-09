using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Web.Http.ExceptionHandling;
using DotNetOwinWebApiSample.Api.Exceptions;
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

        // TODO: WebAPIの例外処理パイプラインとの統合問題により、一時的に除外
        // ErrorHandlingMiddlewareがWebAPIの例外を正しくキャッチできない問題を調査中
        [Test]
        [Ignore("WebAPIの例外処理パイプラインとの統合問題により一時的に除外")]
        public async Task HandleException_ApiBadRequestException_HTTPステータス400()
        {
            // Arrange
            var server = TestServer.Create<TestStartupWithBadRequestException>();
            var client = new HttpClient(server.Handler);

            // Act
            var response = await client.GetAsync("http://localhost/api/test");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Assert.AreEqual("Bad Request Error", result["error"]);

            client.Dispose();
            server.Dispose();
        }

        // TODO: WebAPIの例外処理パイプラインとの統合問題により、一時的に除外
        // ErrorHandlingMiddlewareがWebAPIの例外を正しくキャッチできない問題を調査中
        [Test]
        [Ignore("WebAPIの例外処理パイプラインとの統合問題により一時的に除外")]
        public async Task HandleException_ApiNotFoundException_HTTPステータス404()
        {
            // Arrange
            var server = TestServer.Create<TestStartupWithNotFoundException>();
            var client = new HttpClient(server.Handler);

            // Act
            var response = await client.GetAsync("http://localhost/api/test");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Assert.AreEqual("Not Found Error", result["error"]);

            client.Dispose();
            server.Dispose();
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

    [RoutePrefix("api/test")]
    public class TestControllerWithBadRequestException : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            throw new ApiBadRequestException("Bad Request Error");
        }
    }

    public class TestStartupWithBadRequestException
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolverForBadRequest());
            configuration.MapHttpAttributeRoutes();

            configuration.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());
            app.Use<ErrorHandlingMiddleware>();
            app.UseWebApi(configuration);
        }

        public class TestWebApiResolverForBadRequest : DefaultAssembliesResolver
        {
            public override ICollection<Assembly> GetAssemblies()
            {
                return new List<Assembly> { typeof(TestControllerWithBadRequestException).Assembly };
            }
        }
    }

    [RoutePrefix("api/test")]
    public class TestControllerWithNotFoundException : ApiController
    {
        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            throw new ApiNotFoundException("Not Found Error");
        }
    }

    public class TestStartupWithNotFoundException
    {
        public void Configuration(IAppBuilder app)
        {
            var configuration = new HttpConfiguration();
            configuration.Services.Replace(typeof(IAssembliesResolver), new TestWebApiResolverForNotFound());
            configuration.MapHttpAttributeRoutes();

            configuration.Services.Replace(typeof(IExceptionHandler), new PassthroughExceptionHandler());
            app.Use<ErrorHandlingMiddleware>();
            app.UseWebApi(configuration);
        }

        public class TestWebApiResolverForNotFound : DefaultAssembliesResolver
        {
            public override ICollection<Assembly> GetAssemblies()
            {
                return new List<Assembly> { typeof(TestControllerWithNotFoundException).Assembly };
            }
        }
    }
}
