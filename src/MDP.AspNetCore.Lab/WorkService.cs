using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Lab
{
    public interface WorkService
    {
        // Methods
        void Execute();

        string GetValue();
    }
}
