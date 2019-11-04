using System;
using System.Collections.Generic;
using System.Linq;
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

        public Todo Get(int id)
        {
            return _repository.Get().FirstOrDefault(_ => _.Id == id);
        }

        public Todo Add(string description)
        {
            var id = Get().Max(_ => _.Id) + 1;
            var todo = new Todo
            {
                Id = id,
                Description = description,
                CreatedDate = DateTime.Now
            };
            _repository.Add(todo);
            return todo;
        }

        public void Update(int id, string description)
        {
            var todo = Get(id);
            todo.Description = description;
            _repository.Update(todo);
        }

        public void Remove(int id)
        {
            _repository.Remove(Get(id));
        }
    }
}