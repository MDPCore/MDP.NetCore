using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CLK.Reflection
{
    public static class Type
    {
        // Fields
        private readonly static object _syncLock = new object();

        private static ReadOnlyCollection<System.Type> _typeList = null;


        // Methods
        public static IList<System.Type> FindAllType()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_typeList != null) return _typeList;

                // Assembly
                var assemblyList = CLK.Reflection.Assembly.FindAllAssembly();
                if (assemblyList == null) throw new InvalidOperationException($"{nameof(assemblyList)}=null");
                               
                // TypeList
                var typeList = new List<System.Type>();
                foreach (var assembly in assemblyList)
                {
                    typeList.AddRange(assembly.GetTypes());
                }

                // Attach
                _typeList = typeList.AsReadOnly();

                // Return
                return _typeList;
            }
        }
    }
}
