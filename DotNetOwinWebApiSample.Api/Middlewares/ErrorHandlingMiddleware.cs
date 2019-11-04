using DotNetOwinWebApiSample.Api.Exceptions;
using Microsoft.Owin;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Threading.Tasks;

namespace DotNetOwinWebApiSample.Api.Middlewares
{
    public class ErrorHandlingMiddleware : OwinMiddleware
    {

        public ErrorHandlingMiddleware(OwinMiddleware next) : base(next)
        {
        }

        public override async Task Invoke(IOwinContext context)
        {
            try
            {
                await Next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(IOwinContext context, Exception ex)
        {
            var code = HttpStatusCode.InternalServerError;
            if (ex is ApiBadRequestException) code = HttpStatusCode.BadRequest;
            if (ex is ApiNotFoundException) code = HttpStatusCode.NotFound;

            var result = JsonConvert.SerializeObject(new { error = ex.Message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)code;
            return context.Response.WriteAsync(result);
        }

    }
}