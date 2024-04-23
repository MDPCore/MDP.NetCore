using Microsoft.Extensions.Options;
using MyLab.Todos;
using System;
using System.Linq;
using System.Reflection;
using System.Text.Json;

namespace MDP.ConsoleApp
{
    public class Program
    {
        // Methods
        public static void Run(TodoContext todoContext)
        {
            #region Contracts

            if (todoContext == null) throw new ArgumentException($"{nameof(todoContext)}=null");
            
            #endregion

            // Add
            todoContext.TodoRepository.Add(new Todo()
            {
                TodoId = MDP.Domain.Identifier.NewId(),
                Title = "Hello World",
                IsCompleted = true,
            });

            // FindAll
            var todoList = todoContext.TodoRepository.FindAll();
            if( todoList == null) throw new InvalidOperationException($"{nameof(todoList)}=null");

            // Display
            Console.WriteLine(todoList.FirstOrDefault()?.Title);
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
