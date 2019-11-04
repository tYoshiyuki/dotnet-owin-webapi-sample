using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetOwinWebApiSample.Api.Exceptions
{
    public class ApiBadRequestException : ApiSampleException
    {
        public ApiBadRequestException(string message) : base(message) { }
    }
}