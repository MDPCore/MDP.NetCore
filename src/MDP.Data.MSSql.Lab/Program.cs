using System;

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

            // SqlClient
            using (var sqlClient = sqlClientFactory.CreateClient("DefaultDatabase"))
            {
                // CommandText
                sqlClient.CommandText = "SELECT * FROM [dbo].[Users]";

                // CommandParameters

                // Execute
                using (var reader = sqlClient.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine($"UserId: {reader["UserId"]}, Name: {reader["Name"]}, Mail: {reader["Mail"]}");
                    }
                }
            }
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }
    }
}
