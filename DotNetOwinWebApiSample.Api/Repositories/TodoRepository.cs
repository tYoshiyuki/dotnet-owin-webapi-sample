using System;
using System.Collections.Generic;
using System.Linq;
using DotNetOwinWebApiSample.Api.Models;

namespace DotNetOwinWebApiSample.Api.Repositories
{
    public class TodoRepository : IRepository<Todo>
    {
        private static List<Todo> _todoList = new List<Todo>
        {
            new Todo {Id = 0, Description = "Sample000", CreatedDate = DateTime.Now},
            new Todo {Id = 1, Description = "Sample001", CreatedDate = DateTime.Now},
            new Todo {Id = 2, Description = "Sample002", CreatedDate = DateTime.Now},
            new Todo {Id = 3, Description = "Sample003", CreatedDate = DateTime.Now}
        };

        public void Add(Todo entity)
        {
            if (_todoList.All(_ => _.Id != entity.Id)) _todoList.Add(entity);
        }

        public IEnumerable<Todo> Get()
        {
            return _todoList;
        }

        public void Remove(Todo entity)
        {
            _todoList = _todoList.Where(_ => _.Id != entity.Id).ToList();
        }

        public void Update(Todo entity)
        {
            var target = _todoList.FirstOrDefault(_ => _.Id == entity.Id);
            if (target == null) return;
            target.Description = entity.Description;
            target.CreatedDate = entity.CreatedDate;
        }
    }
}