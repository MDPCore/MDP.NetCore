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
        public TokenProvider CreateProvider(string name)
        {
            #region Contracts

            if (string.IsNullOrEmpty(name) == true) throw new ArgumentException($"{nameof(name)}=null");

            #endregion

            // TokenProviderBuilder
            TokenProviderBuilder builder = null;
            if (_builderDictionary.ContainsKey(name) == true) builder = _builderDictionary[name];
            if (builder == null) throw new InvalidOperationException($"{nameof(builder)}=null");

            // TokenProvider
            var tokenProvider = builder.CreateProvider();
            if (tokenProvider == null) throw new InvalidOperationException($"{nameof(tokenProvider)}=null");

            // Return
            return tokenProvider;
        }
    }
}
