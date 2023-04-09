using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Tracing
{
    public interface ITracer
    {
        // Methods
        TracerActivity Start([CallerMemberName] string memberName = "");
    }

    public interface ITracer<TCategoryName> : ITracer
    {

    }
}
