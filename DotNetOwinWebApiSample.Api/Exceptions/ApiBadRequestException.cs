namespace DotNetOwinWebApiSample.Api.Exceptions
{
    public class ApiBadRequestException : ApiSampleException
    {
        public ApiBadRequestException(string message) : base(message)
        {
        }
    }
}