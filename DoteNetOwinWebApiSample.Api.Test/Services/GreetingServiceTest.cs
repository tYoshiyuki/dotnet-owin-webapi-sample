﻿using DotNetOwinWebApiSample.Api.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DoteNetOwinWebApiSample.Api.Test.Services
{
    [TestClass]
    [TestCategory("Greeting"), TestCategory("Logic")]
    public class GreetingServiceTest
    {
        private static GreetingService _service;

        [ClassInitialize]
        public static void Setup(TestContext context)
        {
            _service = new GreetingService();
        }

        [TestMethod]
        public void Get_正常系()
        {
            // Arrange・Act
            var result = _service.Greeting();

            // Assert
            Assert.AreEqual("Hello", result);
        }
    }
}