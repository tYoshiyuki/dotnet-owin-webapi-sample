using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DotNetOwinWebApiSample.Api.Services;

namespace DotNetOwinWebApiSample.Api.Controllers
{
    [RoutePrefix("api/greeting")]
    public class GreetingController : ApiController
    {
        private readonly GreetingService _service;

        public GreetingController(GreetingService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route]
        public IHttpActionResult Get()
        {
            return Ok(_service.Greeting());
        }

        [HttpGet]
        [Route("{hour:int}")]
        public IHttpActionResult Get(int hour)
        {
            return Ok(_service.Greeting(hour));
        }
    }
}