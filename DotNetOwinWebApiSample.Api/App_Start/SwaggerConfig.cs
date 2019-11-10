using System.Web.Http;
using Swashbuckle.Application;

namespace DotNetOwinWebApiSample.Api
{
    public class SwaggerConfig
    {
        public static void Register(HttpConfiguration configuration)
        {
            var thisAssembly = typeof(SwaggerConfig).Assembly;

            configuration
                .EnableSwagger(c => { c.SingleApiVersion("v1", "DotNetOwinWebApiSample.Api"); })
                .EnableSwaggerUi(c => { });
        }
    }
}