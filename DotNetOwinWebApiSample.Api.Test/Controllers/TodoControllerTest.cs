using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Test.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DotNetOwinWebApiSample.Api.Test.Controllers
{
    [TestClass]
    [TestCategory("Todo"), TestCategory("Integration")]
    public class TodoControllerTest : IntegrationTestBase
    {
        private readonly string _url = "http://localhost/api/todo";

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            Before();
        }

        [ClassCleanup]
        public static void TearDown()
        {
            After();
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

        [TestMethod]
        public async Task Get_by_id_正常系()
        {
            // Arrange・Act
            var expected = 1;
            var response = await HttpClient.GetAsync(_url + $"/{expected}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<Todo>();
            Assert.IsNotNull(result);
            Assert.AreEqual(expected, result.Id);
        }
    }
}