using System;
using System.Linq;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using MyLab.Todos;

namespace MDP.WebApp
{
    public class HomeController : Controller
    {
        // Fields
        private readonly TodoContext _todoContext = null;


        // Constructors
        public HomeController(TodoContext todoContext)
        {
            #region Contracts

            if (todoContext == null) throw new ArgumentException($"{nameof(todoContext)}=null");

            #endregion

            // Default
            _todoContext = todoContext;
        }


        // Methods
        public ActionResult Index()
        {
            // Add
            _todoContext.TodoRepository.Add(new Todo()
            {
                TodoId = MDP.Domain.Identifier.NewId(),
                Title = "Hello World",
                IsCompleted = true,
            });

            // FindAll
            var todoList = _todoContext.TodoRepository.FindAll();
            if (todoList == null) throw new InvalidOperationException($"{nameof(todoList)}=null");

            // ViewBag
            this.ViewBag.Message = todoList.FirstOrDefault()?.Title;

            // Return
            return View();
        }
    }
}
