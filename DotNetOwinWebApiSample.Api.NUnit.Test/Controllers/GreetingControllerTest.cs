using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using DotNetOwinWebApiSample.Api.NUnit.Test.Shared;
using Newtonsoft.Json;
using NUnit.Framework;

namespace DotNetOwinWebApiSample.Api.NUnit.Test.Controllers
{
    [Category("Greeting"), Category("Integration")]
    public class GreetingControllerTest : IntegrationTestBase
    {
        private readonly string _url = "http://localhost/api/greeting";

        [OneTimeSetUp]
        public static void Setup()
        {
            Before();
        }

        [OneTimeTearDown]
        public static void TearDown()
        {
            After();
        }

        [Test]
        public async Task Get_正常系()
        {
            // Arrange・Act
            var response = await HttpClient.GetAsync(_url);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<string>();
            Assert.AreEqual("Hello", result);
        }

        [TestCase(0, "Good evening")]
        [TestCase(4, "Good evening")]
        [TestCase(5, "Good morning")]
        [TestCase(11, "Good morning")]
        [TestCase(12, "Good afternoon")]
        [TestCase(17, "Good afternoon")]
        [TestCase(18, "Good evening")]
        [TestCase(23, "Good evening")]
        public async Task Get_by_hour_正常系(int hour, string expected)
        {
            // Arrange・Act
            var response = await HttpClient.GetAsync(_url + $"/{hour}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<string>();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public async Task Get_by_hour_24以上_ApiBadRequestException()
        {
            // Arrange
            var hour = 24;

            // Act
            var response = await HttpClient.GetAsync(_url + $"/{hour}");

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var result = await response.Content.ReadAsStringAsync();
            var error = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
            Assert.IsTrue(error.ContainsKey("error"));
        }
    }
}
