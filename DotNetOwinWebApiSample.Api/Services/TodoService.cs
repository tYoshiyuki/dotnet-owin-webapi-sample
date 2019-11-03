using System.Collections.Generic;
using DotNetOwinWebApiSample.Api.Models;
using DotNetOwinWebApiSample.Api.Repositories;

namespace DotNetOwinWebApiSample.Api.Services
{
    public class TodoService
    {
        private readonly IRepository<Todo> _repository;
        public TodoService(IRepository<Todo> repository)
        {
            _repository = repository;
        }

        public IEnumerable<Todo> Get()
        {
            return _repository.Get();
        }
    }
}