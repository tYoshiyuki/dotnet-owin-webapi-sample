using Microsoft.Owin.Testing;
using System.Net.Http;

namespace DotNetOwinWebApiSample.Api.Test.Shared
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
