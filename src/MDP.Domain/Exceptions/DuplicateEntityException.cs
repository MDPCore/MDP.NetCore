using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Domain
{
    public class DuplicateEntityException : DomainException
    {
        // Constructors
        public DuplicateEntityException() : base("無法創建或更新實體，因為已經存在具有相同識別的實體。") { }

        public DuplicateEntityException(string message) : base(message) { }

        public DuplicateEntityException(string message, Exception innerException) : base(message, innerException) { }
    }
}
