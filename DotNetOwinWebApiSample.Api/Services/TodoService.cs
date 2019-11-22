using System;
using System.Collections.Generic;
using System.Linq;
using DotNetOwinWebApiSample.Api.Exceptions;
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
            return _repository.Get().FirstOrDefault(_ => _.Id == id) ?? throw new ApiNotFoundException("対象が存在しません。");
        }

        public Todo Add(string description)
        {
            var id = Get().Max(_ => _.Id) + 1;
            var todo = new Todo
            {
                Id = id,
                Description = description ?? throw new ApiBadRequestException("descriptionが未入力です。"),
                CreatedDate = DateTime.Now
            };
            _repository.Add(todo);
            return todo;
        }

        public void Update(int id, string description)
        {
            var todo = Get(id);
            if (todo == null) throw new ApiNotFoundException("対象が存在しません。");

            todo.Description = description ?? throw new ApiBadRequestException("descriptionが未入力です。");
            _repository.Update(todo);
        }

        public void Update(Todo todo)
        {
            if (todo == null) throw new ApiBadRequestException("todoがnullです。");
            Update(todo.Id, todo.Description);
        }

        public void Remove(int id)
        {
            var todo = Get(id);
            if (todo == null) throw new ApiNotFoundException("対象が存在しません。");

            _repository.Remove(Get(id));
        }
    }
}