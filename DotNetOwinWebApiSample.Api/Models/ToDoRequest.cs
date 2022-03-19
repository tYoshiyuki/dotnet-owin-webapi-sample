using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DotNetOwinWebApiSample.Api.Models
{
    public class ToDoPostRequest
    {
        public int Id { get; set; }
        public string Description { get; set; }
    }
}