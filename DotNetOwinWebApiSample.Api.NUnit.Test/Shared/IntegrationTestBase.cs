using System.Net.Http;
using Microsoft.Owin.Testing;

namespace DotNetOwinWebApiSample.Api.NUnit.Test.Shared
{
    public abstract class IntegrationTestBase
    {
        protected static TestServer Server { get; set; }
        protected static HttpClient HttpClient { get; set; }

        protected static void Before()
        {
            Server = TestServer.Create<Startup>();
            HttpClient = Server.HttpClient;
        }

        protected static void After()
        {
            Server.Dispose();
        }
    }
}
