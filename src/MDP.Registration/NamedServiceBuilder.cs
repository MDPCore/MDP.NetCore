using System;

namespace MDP.Registration
{
    public interface NamedServiceBuilder
    {
        // Properties
        string InstanceName { get; }


        // Methods
        object Resolve(IServiceProvider serviceProvider);
    }

    public class NamedServiceBuilder<TService> : NamedServiceBuilder
        where TService : class
    {
        // Fields
        private readonly string _instanceName;

        private readonly Func<IServiceProvider, object> _resolveAction;

        private object _instance = null;


        // Constructors
        public NamedServiceBuilder(string instanceName, Func<IServiceProvider, object> resolveAction)
        {
            #region Contracts

            if (string.IsNullOrEmpty(instanceName) == true) throw new ArgumentException($"{nameof(instanceName)}=null");
            if (resolveAction == null) throw new ArgumentException($"{nameof(resolveAction)}=null");

            #endregion

            // Default
            _instanceName = instanceName;
            _resolveAction = resolveAction;
        }


        // Properties
        public string InstanceName { get { return _instanceName; } }


        // Methods
        public object Resolve(IServiceProvider serviceProvider)
        {
            #region Contracts

            if (serviceProvider == null) throw new ArgumentException($"{nameof(serviceProvider)}=null");

            #endregion

            // Require
            if (_instance != null) return _instance;

            // Resolve
            var instance = _resolveAction(serviceProvider);
            if (instance == null) throw new InvalidOperationException($"{nameof(instance)}=null");

            // Attach
            _instance = instance;

            // Return
            return instance;
        }
    }
}