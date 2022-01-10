using DotNetOwinWebApiSample.Api.Services;
using NUnit.Framework;

namespace DotNetOwinWebApiSample.Api.NUnit.Test.Services
{
    [Category("Logic"), Category("Greeting")]
    public class GreetingServiceTest
    {
        private static GreetingService _service;

        [OneTimeSetUp]
        public static void Setup()
        {
            _service = new GreetingService();
        }

        [Test]
        public void Get_正常系()
        {
            // Arrange・Act
            var result = _service.Greeting();

            // Assert
            Assert.AreEqual("Hello", result);
        }
    }
}
