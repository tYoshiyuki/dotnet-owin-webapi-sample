namespace DotNetOwinWebApiSample.Api.Exceptions
{
    public class ApiNotFoundException : ApiSampleException
    {
        public ApiNotFoundException(string message) : base(message)
        {
        }
    }
}