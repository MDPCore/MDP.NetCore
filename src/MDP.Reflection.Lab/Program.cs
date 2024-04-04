using System;
using System.Diagnostics;

namespace MDP.Reflection.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
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
        }
    }
}
