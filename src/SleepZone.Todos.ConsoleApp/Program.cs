using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

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
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}