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
                sqlClient.CommandText = "SELECT * FROM [dbo].[Users_Users]";

                // CommandParameters

                // Execute
                var userList = sqlClient.ExecuteParseAll<User>();
                if (userList == null) throw new InvalidOperationException($"{nameof(userList)}=null");

                // Display
                foreach(var user in userList)
                {
                    Console.WriteLine($"UserId: {user.UserId}, Name: {user.Name}, Mail: {user.Mail}");
                }
            }
        }

        public static void Main(string[] args)
        {
            // Host
            MDP.NetCore.Host.Run<Program>(args);
        }


        // Class
        public class User
        {
            // Properties
            public string UserId { get; set; }

            public string Name { get; set; }

            public string Mail { get; set; }
        }
    }
}
