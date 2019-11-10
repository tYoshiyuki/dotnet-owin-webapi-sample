using System.Runtime.ExceptionServices;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.ExceptionHandling;

namespace DotNetOwinWebApiSample.Api.ExceptionHandler
{
    /// <summary>
    ///     例外を処理せずにスローするExceptionHandlerです
    /// </summary>
    public class PassthroughExceptionHandler : IExceptionHandler
    {
        public Task HandleAsync(ExceptionHandlerContext context, CancellationToken cancellationToken)
        {
            ExceptionDispatchInfo.Capture(context.Exception).Throw();
            return Task.CompletedTask;
        }
    }
}