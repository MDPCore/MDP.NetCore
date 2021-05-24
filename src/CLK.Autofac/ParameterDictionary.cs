using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Autofac
{
    public interface ParameterDictionary
    {
        // Methods 
        object GetValue(string key, Type type);
    }
}
