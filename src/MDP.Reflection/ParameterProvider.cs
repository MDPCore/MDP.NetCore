using System;
using System.Reflection;

namespace MDP.Reflection
{
    public abstract class ParameterProvider
    {
        // Methods
        public virtual object GetValue(System.Type parameterType, string parameterName, bool hasDefaultValue = false, object defaultValue = null)
        {
            #region Contracts

            if (parameterType == null) throw new ArgumentException($"{nameof(parameterType)}=null");
            if (string.IsNullOrEmpty(parameterName) == true) throw new ArgumentException($"{nameof(parameterName)}=null");

            #endregion

            // ReferenceType
            if (parameterType.IsClass == true || parameterType.IsInterface == true)
            {
                // HasDefaultValue
                if (hasDefaultValue == true)
                {
                    // Return
                    return defaultValue;
                }

                // CreateDefaultValue
                if (parameterType.IsClass == true && parameterType.IsAbstract == false)
                {
                    if( parameterType.GetConstructor(BindingFlags.Public | BindingFlags.Instance, null, System.Type.EmptyTypes, null) != null)
                    {
                        // Create
                        var parameter = Activator.CreateInstance(parameterType);
                        if (parameter != null)
                        {
                            // Return
                            return parameter;
                        }
                    }
                }

                // NullValue
                return null;
            }

            // ValueType
            if (parameterType.IsValueType == true || parameterType == typeof(string))
            {
                // HasDefaultValue
                if (hasDefaultValue == true)
                {
                    // Return
                    return defaultValue;
                }

                // CreateDefaultValue
                {
                    // Create
                    var parameter = Activator.CreateInstance(parameterType);
                    if (parameter != null)
                    {
                        // Return
                        return parameter;
                    }
                }

                // NullValue
                return null;
            }

            // UnknownType
            return null;
        }
    }
}
