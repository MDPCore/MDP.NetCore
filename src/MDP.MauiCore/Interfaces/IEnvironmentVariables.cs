using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.MauiCore
{
    public interface IEnvironmentVariables
    {
        // Methods
        string GetVariable(string name);
    }
}
