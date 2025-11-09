using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Test.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

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

        [TestMethod]
        public async Task Get_by_id_存在しないID_ApiNotFoundException()
        {
            // Arrange・Act
            var id = 999;
            var response = await HttpClient.GetAsync(_url + $"/{id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Assert.IsTrue(result.ContainsKey("error"));
            Assert.AreEqual("対象が存在しません。", result["error"]);
        }

        [TestMethod]
        public async Task Post_正常系()
        {
            // Arrange
            var todo = new ToDoPostRequest { Id = 1, Description = "Updated Todo" };
            var content = new StringContent(
                JsonConvert.SerializeObject(todo),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await HttpClient.PostAsync(_url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<Todo>();
            Assert.AreEqual(todo.Id, result.Id);
            Assert.AreEqual(todo.Description, result.Description);
        }

        [TestMethod]
        public async Task Post_存在しないID_ApiNotFoundException()
        {
            // Arrange
            var todo = new ToDoPostRequest { Id = 999, Description = "Not Found Todo" };
            var content = new StringContent(
                JsonConvert.SerializeObject(todo),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await HttpClient.PostAsync(_url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Assert.IsTrue(result.ContainsKey("error"));
            Assert.AreEqual("対象が存在しません。", result["error"]);
        }

        [TestMethod]
        public async Task Post_descriptionがnull_ApiBadRequestException()
        {
            // Arrange
            var todo = new ToDoPostRequest { Id = 1, Description = null };
            var content = new StringContent(
                JsonConvert.SerializeObject(todo),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await HttpClient.PostAsync(_url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Assert.IsTrue(result.ContainsKey("error"));
            Assert.AreEqual("descriptionが未入力です。", result["error"]);
        }

        [TestMethod]
        public async Task Put_正常系()
        {
            // Arrange
            var todo = new ToDoPutRequest { Description = "New Todo" };
            var content = new StringContent(
                JsonConvert.SerializeObject(todo),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await HttpClient.PutAsync(_url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadAsAsync<Todo>();
            Assert.IsNotNull(result);
            Assert.AreEqual(todo.Description, result.Description);
        }

        [TestMethod]
        public async Task Put_descriptionがnull_ApiBadRequestException()
        {
            // Arrange
            var todo = new ToDoPutRequest { Description = null };
            var content = new StringContent(
                JsonConvert.SerializeObject(todo),
                Encoding.UTF8,
                "application/json");

            // Act
            var response = await HttpClient.PutAsync(_url, content);

            // Assert
            Assert.AreEqual(HttpStatusCode.BadRequest, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Assert.IsTrue(result.ContainsKey("error"));
            Assert.AreEqual("descriptionが未入力です。", result["error"]);
        }

        [TestMethod]
        public async Task Delete_正常系()
        {
            // Arrange
            // まず既存のTodoを取得して、削除可能なIDを確認
            var getResponse = await HttpClient.GetAsync(_url);
            var todos = await getResponse.Content.ReadAsAsync<List<Todo>>();
            var idToDelete = todos.First().Id;

            // Act
            var response = await HttpClient.DeleteAsync(_url + $"/{idToDelete}");

            // Assert
            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
        }

        [TestMethod]
        public async Task Delete_存在しないID_ApiNotFoundException()
        {
            // Arrange
            var id = 999;

            // Act
            var response = await HttpClient.DeleteAsync(_url + $"/{id}");

            // Assert
            Assert.AreEqual(HttpStatusCode.NotFound, response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);
            Assert.IsTrue(result.ContainsKey("error"));
            Assert.AreEqual("対象が存在しません。", result["error"]);
        }
    }
}