using System;

namespace MDP.Hosting
{
    internal interface ParameterProvider
    {
        // Methods
        bool Exists(string parameterName);

        object GetValue(string parameterName, Type parameterType);
    }
}
