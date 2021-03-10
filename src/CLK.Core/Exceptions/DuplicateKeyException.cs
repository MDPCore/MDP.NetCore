using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CLK
{
    public class DuplicateKeyException : Exception
    {
        // Constructors
        public DuplicateKeyException() : base("主鍵衝突") { }

        public DuplicateKeyException(string message) : base(message) { }

        public DuplicateKeyException(string message, Exception innerException) : base(message, innerException) { }
    }
}
