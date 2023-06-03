using Microsoft.Extensions.Hosting;

namespace MDP.Data.MSSql.Lab
{
    public class Program
    {
        // Methods
        public static void Run(SqlClientFactory sqlClientFactory)
        {
            #region Contracts

            if (sqlClientFactory == null) throw new ArgumentException($"{nameof(sqlClientFactory)}=null");

            #endregion

            // Execute
            Console.WriteLine(sqlClientFactory.ToString());
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Create<Program>(args).Run();
        }
    }
}
