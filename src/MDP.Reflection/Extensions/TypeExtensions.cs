using System;
using System.Reflection;
using System.Linq;

namespace MDP.Reflection
{
    public static class TypeExtensions
    {
        // Methods
        public static ConstructorInfo FindConstructor(this System.Type type)
        {
            #region Contracts

            if (type == null) throw new ArgumentException(nameof(type));

            #endregion

            // Require
            if (type.IsAbstract == true) throw new InvalidOperationException($"The '{type.FullName}' is abstract.");

            // ConstructorInfoList
            var constructorInfoList = type.GetConstructors().ToList();
            if (constructorInfoList.Count == 0) throw new InvalidOperationException($"Not having constructor in the type '{type.FullName}' is not supported.");
            if (constructorInfoList.Count >= 2) throw new InvalidOperationException($"Having multiple constructor in the type '{type.FullName}' is not supported.");

            // ConstructorInfo
            var constructorInfo = constructorInfoList.First();
            if (constructorInfo == null) throw new InvalidOperationException($"{nameof(constructorInfo)}=null");

            // Return
            return constructorInfo;
        }

        public static MethodInfo FindMethod(this System.Type type, string methodName)
        {
            #region Contracts

            if (type == null) throw new ArgumentException(nameof(type));
            if (string.IsNullOrEmpty(methodName) == true) throw new ArgumentException(nameof(methodName));

            #endregion

            // MethodInfoList
            var methodInfoList = type.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static).Where(method => method.Name.Equals(methodName, StringComparison.OrdinalIgnoreCase)).ToList();
            if (methodInfoList.Count == 0) return null;
            if (methodInfoList.Count >= 2) throw new InvalidOperationException($"Having multiple overloads of method '{methodName}' in the type '{type.FullName}' is not supported.");

            // MethodInfo
            var methodInfo = methodInfoList.First();
            if (methodInfo == null) throw new InvalidOperationException($"{nameof(methodInfo)}=null");

            // Return
            return methodInfo;
        }
    }
}
