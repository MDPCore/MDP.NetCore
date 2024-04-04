﻿using Azure.Identity;
using Azure.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MDP.Registration;

namespace MDP.Data.MSSql.Azure
{
    [Service<SqlClientHandler>(singleton: true, autoRegister: false)]
    public class AzureCredentialHandler : SqlClientHandler
    {
        // Fields
        private readonly object _lockObject = new object();

        private readonly TokenCredential _credential;

        private readonly string[] _scopes = null;

        private AccessToken _accessToken = new AccessToken();


        // Constructors
        public AzureCredentialHandler(TokenCredential credential, List<string> scopes)
        {
            #region Contracts

            if (credential == null) throw new ArgumentException($"{nameof(credential)}=null");
            if (scopes == null) throw new ArgumentException($"{nameof(scopes)}=null");

            #endregion

            // Default
            _credential = credential;
            _scopes = scopes.ToArray();
        }


        // Methods
        public void Handle(SqlClient sqlClient)
        {
            #region Contracts

            if (sqlClient == null) throw new ArgumentException($"{nameof(sqlClient)}=null");

            #endregion

            // Variables
            AccessToken accessToken;

            // Sync
            lock (_lockObject)
            {
                // AccessToken
                accessToken = _accessToken;
                if (accessToken.ExpiresOn <= DateTimeOffset.UtcNow)
                {
                    // GetToken
                    accessToken = _credential.GetToken(new TokenRequestContext(_scopes), CancellationToken.None);
                    if (accessToken.Token == null) throw new InvalidOperationException($"{nameof(accessToken)}=null");
                    if (accessToken.Token == String.Empty) throw new InvalidOperationException($"{nameof(accessToken)}=null");
                    if (accessToken.ExpiresOn <= DateTimeOffset.UtcNow) throw new InvalidOperationException($"{nameof(accessToken)}=null");

                    // Set
                    _accessToken = accessToken;
                }
            }

            // Connection.AccessToken
            sqlClient.Connection.AccessToken = accessToken.Token;           
        }
    }
}
