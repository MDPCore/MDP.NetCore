using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Transactions
{
    public interface TransactionFactory : IDisposable
    {
        // Constructors
        void Start();


        // Methods
        Transaction Create();
    }
}
