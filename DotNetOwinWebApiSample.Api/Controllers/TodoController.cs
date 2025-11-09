using System.Linq;
using System.Web.Http;
using DotNetOwinWebApiSample.Api.Models;
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
        public IHttpActionResult Post([FromBody] ToDoPostRequest todo)
        {
            _service.Update(todo.Id, todo.Description);
            var target = _service.Get(todo.Id);
            return Ok(target);
        }

        [HttpPut]
        [Route]
        public IHttpActionResult Put([FromBody] ToDoPutRequest todo)
        {
            var target = _service.Add(todo.Description);
            return Ok(target);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public IHttpActionResult Delete(int id)
        {
            _service.Remove(id);
            return Ok();
        }
    }
}