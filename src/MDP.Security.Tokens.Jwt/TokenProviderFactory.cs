using System;
using System.Collections.Generic;
using System.Linq;

namespace MDP.Security.Tokens.Jwt
{
    public class TokenProviderFactory
    {
        // Fields
        private readonly Dictionary<string, TokenProviderBuilder> _builderDictionary;


        // Constructors
        public TokenProviderFactory(IList<TokenProviderBuilder> builderList)
        {
            #region Contracts

            if (builderList == null) throw new ArgumentException(nameof(builderList));

            #endregion

            // Default
            _builderDictionary = builderList.ToDictionary(o => o.Name, o => o, StringComparer.OrdinalIgnoreCase);
        }


        // Methods
        public TokenProvider CreateProvider(string name = null)
        {
            // TokenProviderBuilder
            TokenProviderBuilder builder = null;
            if (string.IsNullOrEmpty(name) == true)
            {
                builder = _builderDictionary.Values.FirstOrDefault();
            }
            if (string.IsNullOrEmpty(name) == false && _builderDictionary.ContainsKey(name) == true)
            {
                builder = _builderDictionary[name];
            }
            if (builder == null) return null;

            // TokenProvider
            var tokenProvider = builder.CreateProvider();
            if (tokenProvider == null) throw new InvalidOperationException($"{nameof(tokenProvider)}=null");

            // Return
            return tokenProvider;
        }
    }
}
