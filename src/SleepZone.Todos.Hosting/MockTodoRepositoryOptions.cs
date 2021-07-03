using Autofac;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Hosting
{
    public class MockTodoRepositoryOptions
    {
        // Properties
        public List<Todo> TodoList { get; set; } = new List<Todo>();
    }
}
