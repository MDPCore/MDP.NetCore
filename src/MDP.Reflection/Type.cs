using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Reflection
{
    public static class Type
    {
        // Fields
        private readonly static object _syncLock = new object();

        private static ReadOnlyCollection<System.Type> _applicationTypeList = null;


        // Methods
        public static IList<System.Type> FindAllApplicationType()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_applicationTypeList != null) return _applicationTypeList;

                // ApplicationAssemblyList
                var applicationAssemblyList = MDP.Reflection.Assembly.FindAllApplicationAssembly();
                if (applicationAssemblyList == null) throw new InvalidOperationException($"{nameof(applicationAssemblyList)}=null");

                // ApplicationTypeList
                var applicationTypeList = applicationAssemblyList.AsParallel().SelectMany(applicationAssembly =>
                {
                    // TypeList
                    var typeList = applicationAssembly.GetTypes();
                    if (typeList == null) return null;

                    // Return
                    return typeList;
                }).ToList();

                // Attach
                _applicationTypeList = applicationTypeList.AsReadOnly();

                // Return
                return _applicationTypeList;
            }
        }
    }
}
