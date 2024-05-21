using System;
using System.Diagnostics;
using System.Threading.Tasks;

namespace MDP.Reflection.Lab
{
    public class Program
    {
        // Methods
        public static async Task Main(string[] args)
        {
            // ApplicationAssemblyList
            var applicationAssemblyList = MDP.Reflection.Assembly.FindAllApplicationAssembly();
            if (applicationAssemblyList == null) throw new InvalidOperationException($"{nameof(applicationAssemblyList)}=null");
            foreach (var applicationAssembly in applicationAssemblyList)
            {
                Console.WriteLine(applicationAssembly.FullName);
            }
            Console.WriteLine("\n\n");

            // ApplicationTypeList
            var applicationTypeList = MDP.Reflection.Type.FindAllApplicationType();
            if (applicationTypeList == null) throw new InvalidOperationException($"{nameof(applicationTypeList)}=null");
            foreach (var applicationType in applicationTypeList)
            {
                Console.WriteLine(applicationType.FullName);
            }
            Console.WriteLine("\n\n");

            // Activator
            var messageService = Activator.CreateInstance(typeof(MessageService)) as MessageService;
            {
                Console.WriteLine(Activator.InvokeMethod(messageService, "GetValue"));
                Console.WriteLine(Activator.InvokeMethod(messageService, "GetValueAsync"));
                Console.WriteLine(await Activator.InvokeMethodAsync(messageService, "GetValue"));
                Console.WriteLine(await Activator.InvokeMethodAsync(messageService, "GetValueAsync"));
            }
            Console.WriteLine("\n\n");
        }


        // Class
        public class MessageService 
        {
            // Methods
            public string GetValue()
            {
                // Return
                return "Hello World";
            }

            public Task<string> GetValueAsync()
            {
                // Return
                return Task.FromResult("Hello World");
            }
        }
    }
}
