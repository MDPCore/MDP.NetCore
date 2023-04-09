using Autofac;
using MDP.Registration;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Hosting
{
    public abstract class RegisterContext
    {
        // Fields
        private readonly static object _syncLock = new object();

        private static List<Assembly>? _moduleAssemblyList = null;

        private static List<Type>? _moduleTypeList = null;


        // Methods
        protected void RegisterService<T1>(T1 t1)
        {
            #region Contracts

            if (t1 == null) throw new ArgumentException($"{nameof(t1)}=null");

            #endregion

            // RegisterFactory
            this.FindAllRegisterFactory().OfType<RegisterFactory<T1>>().ToList().ForEach(registerFactory => 
            {
                // RegisterService
                registerFactory.RegisterService(t1);
            });           
        }

        protected void RegisterService<T1, T2>(T1 t1, T2 t2)
        {
            #region Contracts

            if (t1 == null) throw new ArgumentException($"{nameof(t1)}=null");
            if (t2 == null) throw new ArgumentException($"{nameof(t2)}=null");

            #endregion

            // RegisterFactory
            this.FindAllRegisterFactory().OfType<RegisterFactory<T1, T2>>().ToList().ForEach(registerFactory =>
            {
                // RegisterService
                registerFactory.RegisterService(t1, t2);
            });
        }

        protected void RegisterService<T1, T2, T3>(T1 t1, T2 t2, T3 t3)
        {
            #region Contracts

            if (t1 == null) throw new ArgumentException($"{nameof(t1)}=null");
            if (t2 == null) throw new ArgumentException($"{nameof(t2)}=null");
            if (t3 == null) throw new ArgumentException($"{nameof(t3)}=null");

            #endregion

            // RegisterFactory
            this.FindAllRegisterFactory().OfType<RegisterFactory<T1, T2, T3>>().ToList().ForEach(registerFactory =>
            {
                // RegisterService
                registerFactory.RegisterService(t1, t2, t3);
            });
        }


        protected abstract RegisterFactory? CreateRegisterFactory(Type moduleType);

        private List<RegisterFactory> FindAllRegisterFactory()
        {
            // ModuleTypeList
            var moduleTypeList = this.FindAllModuleType();
            if (moduleTypeList == null) throw new InvalidOperationException($"{nameof(moduleTypeList)}=null");

            // RegisterFactoryList
            var registerFactoryList = new List<RegisterFactory>();
            foreach ( var moduleType in moduleTypeList )
            {
                // RegisterFactory
                var registerFactory = this.CreateRegisterFactory(moduleType);
                if (registerFactory == null) continue;

                // Add
                registerFactoryList.Add(registerFactory);
            }

            // Return
            return registerFactoryList;
        }

        private List<Type> FindAllModuleType()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_moduleTypeList != null) return _moduleTypeList;

                // ModuleAssemblyList
                var moduleAssemblyList = this.FindAllModuleAssembly();
                if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");

                // ModuleTypeList
                var moduleTypeList = new List<Type>();              
                foreach (var moduleAssembly in moduleAssemblyList)
                {
                    moduleTypeList.AddRange(moduleAssembly.GetTypes());
                }

                // Attach
                _moduleTypeList = moduleTypeList;

                // Return
                return _moduleTypeList;
            }
        }

        private List<Assembly> FindAllModuleAssembly()
        {
            // Sync
            lock (_syncLock)
            {
                // Require
                if (_moduleAssemblyList != null) return _moduleAssemblyList;

                // ModuleAssembly
                var moduleAssemblyList = CLK.Reflection.Assembly.GetAllAssembly(@"*.dll");
                if (moduleAssemblyList == null) throw new InvalidOperationException($"{nameof(moduleAssemblyList)}=null");
               
                // EntryAssembly
                var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
                if (entryAssembly == null) throw new InvalidOperationException($"{nameof(entryAssembly)}=null");
                if (moduleAssemblyList.Contains(entryAssembly) == false) moduleAssemblyList.Add(entryAssembly);

                // Attach
                _moduleAssemblyList = moduleAssemblyList;

                // Return
                return _moduleAssemblyList;
            }
        }
    }
}
