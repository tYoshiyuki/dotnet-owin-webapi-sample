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
        [Route]
        public IHttpActionResult Get()
        {
            return Ok(_service.Get());
        }

        [HttpGet]
        [Route("{id:int}")]
        public IHttpActionResult Get(int id)
        {
            var target = _service.Get(id);
            return Ok(target);
        }

        [HttpPost]
        [Route]
        public IHttpActionResult Post(int id, string description)
        {
            _service.Update(id, description);
            var target = _service.Get(id);
            return Ok(target);
        }

        [HttpPut]
        [Route]
        public IHttpActionResult Put(string description)
        {
            var target = _service.Add(description);
            return Ok(target);
        }

        [HttpDelete]
        [Route]
        public IHttpActionResult Delete(int id)
        {
            _service.Remove(id);
            return Ok();
        }
    }
}