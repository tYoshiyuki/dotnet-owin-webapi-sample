using System;
using System.Net;
using System.Threading.Tasks;
using DotNetOwinWebApiSample.Api.Exceptions;
using Microsoft.Owin;
using Newtonsoft.Json;

namespace DotNetOwinWebApiSample.Api.Middlewares
{
    /// <summary>
    /// 基底の例外処理を行うミドルウェアです
    /// </summary>
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

        /// <summary>
        /// ロジック内で発生した例外に応じて処理を行います
        /// </summary>
        /// <param name="context">IOwinContext</param>
        /// <param name="ex">Exception</param>
        /// <returns>Task</returns>
        private static Task HandleExceptionAsync(IOwinContext context, Exception ex)
        {
            // 例外に応じてHTTPステータスコードを設定します
            var code = HttpStatusCode.InternalServerError;
            switch (ex)
            {
                case ApiBadRequestException _:
                    code = HttpStatusCode.BadRequest;
                    break;
                case ApiNotFoundException _:
                    code = HttpStatusCode.NotFound;
                    break;
            }

            // エラーメッセージを設定します
            var result = JsonConvert.SerializeObject(new {error = ex.Message});
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int) code;
            return context.Response.WriteAsync(result);
        }
    }
}