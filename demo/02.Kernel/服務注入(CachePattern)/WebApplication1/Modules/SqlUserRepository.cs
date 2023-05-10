using MDP.Registration;
using System;

namespace WebApplication1
{
    [Service<UserRepository>()]
    public class SqlUserRepository : UserRepository
    {
        // Fields
        private readonly string _connectionString;


        // Constructors
        public SqlUserRepository(string connectionString)
        {
            // Default
            _connectionString = connectionString;
        }


        // Methods
        public User? Find()
        {
            // Return
            return new User()
            {
                Name= "Clark-->" + DateTime.Now.ToString()
            };
        }
    }
}
