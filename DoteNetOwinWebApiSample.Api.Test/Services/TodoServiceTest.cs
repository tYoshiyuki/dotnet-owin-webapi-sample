using System;
using System.Collections.Generic;
using System.Linq;
using DotNetOwinWebApiSample.Api.Exceptions;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;
using DotNetOwinWebApiSample.Api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace DoteNetOwinWebApiSample.Api.Test.Services
{
    [TestClass]
    [TestCategory("Todo")]
    [TestCategory("Logic")]
    public class TodoServiceTest
    {
        private readonly List<Todo> _data = new List<Todo>
        {
            new Todo {Id = 1, Description = "Test 001", CreatedDate = DateTime.Now},
            new Todo {Id = 2, Description = "Test 002", CreatedDate = DateTime.Now},
            new Todo {Id = 3, Description = "Test 003", CreatedDate = DateTime.Now},
            new Todo {Id = 4, Description = "Test 004", CreatedDate = DateTime.Now}
        };

        private Mock<IRepository<Todo>> _mock;
        private TodoService _service;

        [TestInitialize]
        public void Before()
        {
            _mock = new Mock<IRepository<Todo>>();
            _service = new TodoService(_mock.Object);
        }

        [TestMethod]
        public void Get_正常系()
        {
            // Arrange
            _mock.Setup(_ => _.Get())
                .Returns(_data);
            _service = new TodoService(_mock.Object);

            // Act
            var result = _service.Get().ToList();

            // Assert
            Assert.AreEqual(4, result.Count);
            CollectionAssert.AreEqual(_data, result);
        }

        [TestMethod]
        public void Get_ById_正常系()
        {
            // Arrange
            _mock.Setup(_ => _.Get())
                .Returns(_data);
            _service = new TodoService(_mock.Object);

            // Act
            var result = _service.Get(1);

            // Assert
            Assert.AreEqual(_data.Find(_ => _.Id == 1), result);
        }

        [TestMethod]
        [ExpectedException(typeof(ApiNotFoundException))]
        public void Get_ById_対象無し()
        {
            // Arrange
            _mock.Setup(_ => _.Get())
                .Returns(_data);
            _service = new TodoService(_mock.Object);

            // Act
            _service.Get(999);

            // Assert
            Assert.Fail("通ってはいけないロジック。");
        }
    }
}