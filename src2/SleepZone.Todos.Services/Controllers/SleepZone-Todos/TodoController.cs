using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SleepZone.Todos.Services
{
    [MDP.AspNetCore.Module("SleepZone-Todos")]
    public partial class TodoController : Controller
    {
        // Fields
        private readonly TodoContext _todoContext = null;


        // Constructors
        public TodoController(TodoContext todoContext)
        {
            #region Contracts

            if (todoContext == null) throw new ArgumentException(nameof(todoContext));

            #endregion

            // Default
            _todoContext = todoContext; 
        }
    }

    public partial class TodoController : Controller
    {
        // Methods
        public ActionResult<AddResultModel> Add([FromBody] AddActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // Add
            var todo = new Todo()
            {
                TodoId = Guid.NewGuid().ToString(),
                Name = actionModel.Name,
                IsComplete = actionModel.IsComplete
            };
            _todoContext.TodoRepository.Add(todo);

            // Return
            return (new AddResultModel()
            {
                Todo = todo
            });
        }


        // Class
        public class AddActionModel
        {
            // Properties
            public string Name { get; set; }

            public bool IsComplete { get; set; }
        }

        public class AddResultModel
        {
            // Properties
            public Todo Todo { get; set; }
        }
    }

    public partial class TodoController : Controller
    {
        // Methods
        public ActionResult<FindAllnResultModel> FindAll([FromBody] FindAllActionModel actionModel)
        {
            #region Contracts

            if (actionModel == null) throw new ArgumentException(nameof(actionModel));

            #endregion

            // Return
            return (new FindAllnResultModel()
            {
                Todos = _todoContext.TodoRepository.FindAll()
            });
        }


        // Class
        public class FindAllActionModel
        {
            // Properties
           
        }

        public class FindAllnResultModel
        {
            // Properties
            public List<Todo> Todos { get; set; }
        }
    }
}
