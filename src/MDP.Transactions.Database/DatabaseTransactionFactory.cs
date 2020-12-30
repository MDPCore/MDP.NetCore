using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MDP.Transactions.Database
{
    public class DatabaseTransactionFactory : TransactionFactory
    {
        // Constructors
        public DatabaseTransactionFactory()
        {

        }

        public void Start()
        {

        }

        public void Dispose()
        {

        }


        // Methods
        public Transaction Create()
        {
            // Return
            return new DatabaseTransaction();
        }
    }
}
