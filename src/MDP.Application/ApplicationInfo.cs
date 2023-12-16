using System;

namespace MDP.Application
{
    public class ApplicationInfo
    {
        // Constructors
        public ApplicationInfo(string name) 
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // Default
            this.Name = name;
        }


        // Properties
        public string Name { get; }
    }
}
