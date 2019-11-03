using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;
using Microsoft.Owin.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace DoteNetOwinWebApiSample.Api.Test.Controllers
{
    [TestClass]
    [TestCategory("Todo"), TestCategory("e2e")]
    public class TodoControllerTest
    {
        private readonly string _url = "http://localhost/api/todo";
        private static TestServer Server { get; set; }
        private static HttpClient Client { get; set; }

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            Server = TestServer.Create<Startup>();
            Client = new HttpClient(Server.Handler);
        }

        [TestMethod]
        public async Task Get_正常系()
        {
            // Arrange
            var expect = TestUtil.Kernel.Get<TodoRepository>().Get().ToList();

            // Act
            var response = await Client.GetAsync(_url);
            var result = await response.Content.ReadAsAsync<List<Todo>>();

            // Assert
            Assert.IsTrue(result.Any());
            Assert.AreEqual(result.Count, expect.Count);
        }

        [ClassCleanup]
        public static void Dispose()
        {
            Client.Dispose();
            Server.Dispose();
        }
    }
}
