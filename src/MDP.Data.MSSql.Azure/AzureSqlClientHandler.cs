using Azure.Identity;
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
    [Service<SqlClientHandler>(singleton: true, autoRegister: true)]
    public class AzureSqlClientHandler : SqlClientHandler
    {
        // Fields
        private readonly object _lockObject = new object();

        private readonly TokenCredential _azureCredential;

        private readonly string[] _scopes = new string[] { "https://database.windows.net/.default" };

        private AccessToken _accessToken = new AccessToken();


        // Constructors
        public AzureSqlClientHandler(TokenCredential azureCredential)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(azureCredential);

            #endregion

            // Default
            _azureCredential = azureCredential;
        }


        // Methods
        public void Handle(SqlClient sqlClient)
        {
            #region Contracts

            ArgumentNullException.ThrowIfNull(sqlClient);

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
                    accessToken = _azureCredential.GetToken(new TokenRequestContext(_scopes), CancellationToken.None);
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
