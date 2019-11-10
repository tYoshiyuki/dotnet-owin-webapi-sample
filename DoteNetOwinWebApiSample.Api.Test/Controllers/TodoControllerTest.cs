using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetOwinWebApiSample.Api;
using DotNetOwinWebApiSample.Api.Models;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoteNetOwinWebApiSample.Api.Test.Controllers
{
    [TestClass]
    [TestCategory("Todo")]
    [TestCategory("Integration")]
    public class TodoControllerTest
    {
        private readonly string _url = "http://localhost/api/todo";
        private static TestServer Server { get; set; }
        private static HttpClient HttpClient { get; set; }

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            Server = TestServer.Create<Startup>();
            HttpClient = Server.HttpClient;
        }

        [TestMethod]
        public async Task Get_正常系()
        {
            // Arrange・Act
            var response = await HttpClient.GetAsync(_url);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<List<Todo>>();
            Assert.IsTrue(result.Any());
        }

        [ClassCleanup]
        public static void Dispose()
        {
            Server.Dispose();
        }
    }
}