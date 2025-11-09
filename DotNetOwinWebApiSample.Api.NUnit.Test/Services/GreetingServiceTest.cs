using DotNetOwinWebApiSample.Api.Exceptions;
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

        [TestCase(0, "Good evening")]
        [TestCase(4, "Good evening")]
        [TestCase(5, "Good morning")]
        [TestCase(11, "Good morning")]
        [TestCase(12, "Good afternoon")]
        [TestCase(17, "Good afternoon")]
        [TestCase(18, "Good evening")]
        [TestCase(23, "Good evening")]
        public void Greeting_by_hour_正常系(int hour, string expected)
        {
            // Arrange・Act
            var result = _service.Greeting(hour);

            // Assert
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void Greeting_by_hour_24以上_ApiBadRequestExceptionが発生()
        {
            // Arrange・Act & Assert
            Assert.That(() => _service.Greeting(24), Throws.TypeOf<ApiBadRequestException>());
        }
    }
}
