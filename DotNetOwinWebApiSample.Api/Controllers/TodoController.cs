using System.Linq;
using System.Web.Http;
using DotNetOwinWebApiSample.Api.Services;

namespace DotNetOwinWebApiSample.Api.Controllers
{
    [RoutePrefix("api/todo")]
    public class TodoController : ApiController
    {
        private readonly TodoService _service;

        public TodoController(TodoService service)
        {
            _service = service;
        }

        [HttpGet]
        [Route("")]
        public IHttpActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var target = _service.Get().FirstOrDefault(_ => _.Id == id);
            if (target == null) return NotFound();
            return Ok(target);
        }

    }
}
