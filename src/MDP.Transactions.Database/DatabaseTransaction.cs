using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;

namespace MDP.Transactions.Database
{
    public class DatabaseTransaction : Transaction
    {
        // Fields
        private static TransactionOptions _transactionOptions = new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted, Timeout = TransactionManager.DefaultTimeout };

        private readonly TransactionScope _transactionScope = null;


        // Constructors
        public DatabaseTransaction()
        {
            // TransactionScope
            _transactionScope = new TransactionScope(TransactionScopeOption.Required, _transactionOptions);
        }

        public void Dispose()
        {
            // TransactionScope
            _transactionScope.Dispose();
        }


        // Methods
        public void Complete()
        {
            // TransactionScope
            _transactionScope.Complete();
        }
    }
}