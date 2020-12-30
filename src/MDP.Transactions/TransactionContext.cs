using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Transactions
{
    public class TransactionContext : IDisposable
    {
        // Fields
        private readonly TransactionFactory _transactionFactory = null;


        // Constructors
        public TransactionContext(TransactionFactory transactionFactory)
        {
            #region Contracts

            if (transactionFactory == null) throw new ArgumentException();

            #endregion

            // Default
            _transactionFactory = transactionFactory;
        }

        public void Start()
        {
            // TransactionFactory
            _transactionFactory.Start();
        }

        public void Dispose()
        {
            // TransactionFactory
            _transactionFactory.Dispose();
        }


        // Methods
        public Transaction BeginTransaction()
        {
            // Transaction
            var transaction = _transactionFactory.Create();
            if (transaction == null) throw new InvalidOperationException("transaction=null");

            // Return
            return transaction;
        }
    }
}
