using System;

namespace MDP.Domain.Lab
{
    public class Program
    {
        // Methods
        public static void Main(string[] args)
        {
            // Identifier
            for (int i = 0; i < 10; i++)
            {
                // NewId
                var identifier = Identifier.NewId();
                if (string.IsNullOrEmpty(identifier) == true) throw new InvalidOperationException($"{nameof(identifier)}=null");

                // Display
                Console.WriteLine($"New Identifier: {identifier}");
            }
        }
    }
}