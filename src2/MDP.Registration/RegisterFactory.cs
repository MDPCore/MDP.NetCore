using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Registration
{
    public interface RegisterFactory
    {
       
    }

    public interface RegisterFactory<T1> : RegisterFactory
    {
        // Methods
        void RegisterService(T1 t1);
    }

    public interface RegisterFactory<T1, T2>: RegisterFactory
    {
        // Methods
        void RegisterService(T1 t1, T2 t2);
    }

    public interface RegisterFactory<T1, T2, T3> : RegisterFactory
    {
        // Methods
        void RegisterService(T1 t1, T2 t2, T3 t3);
    }
}
