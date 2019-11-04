using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetOwinWebApiSample.Api.Exceptions
{
    public class ApiNotFoundException : ApiSampleException
    {
        public ApiNotFoundException(string message) : base(message) { }
    }
}