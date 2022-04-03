using Autofac;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace SleepZone.Todos.ConsoleApp
{
    public class Program
    {
        // Methods
        public static void Run(TodoContext todoContext)
        {
            #region Contracts

            if (todoContext == null) throw new ArgumentException(nameof(todoContext));

            #endregion

            // Todo
            var todo = todoContext.TodoRepository.FindAll().FirstOrDefault();
            if (todo == null) throw new InvalidOperationException($"{nameof(todo)}=null");

            // Display
            Console.WriteLine(todo.Name);
        }

        public static void Main(string[] args)
        {
            // Host
            SleepZone.Todos.ConsolePlatform.Host.Run<Program>(args);
        }
    }
}
