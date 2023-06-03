using System;

namespace MDP.Network.Rest
{
    internal class RestClientDefaults
    {
        // Constants
        public static string PrefixName { get; private set; } = Guid.NewGuid().ToString();


        // Methods
        public static string CreateName(string name)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // Name
            name = $"__{RestClientDefaults.PrefixName}_{name}";

            // Return
            return name;
        }
    }
}