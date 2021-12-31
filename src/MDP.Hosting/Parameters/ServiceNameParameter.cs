using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public class ServiceNameParameter : TypedParameter
    {
        // Constructors
        public ServiceNameParameter(string value) : base(typeof(string), value) { }


        // Properties
        public string ValueString { get { return this.Value as string; } }
    }
}
