using System;

namespace DotNetOwinWebApiSample.Api.Exceptions
{
    public class ApiSampleException : Exception
    {
        public ApiSampleException()
        {
        }

        public ApiSampleException(string message) : base(message)
        {
        }

        public ApiSampleException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}