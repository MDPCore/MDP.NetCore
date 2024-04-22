using System;
using System.Security.Claims;
using System.Threading.Tasks;
using MDP.Registration;
using Microsoft.AspNetCore.Components.Authorization;

namespace MDP.BlazorCore.Maui
{
    [Service<AuthenticationStateProvider>()]
    public class MauiAuthenticationStateProvider : AuthenticationStateProvider
    {
        // Fields
        private UserContext _userContext;


        // Constructors
        public MauiAuthenticationStateProvider(UserContext userContext)
        {
            #region Contracts

            if (userContext == null) throw new ArgumentException($"{nameof(userContext)}=null");

            #endregion

            // Default
            _userContext = userContext;

            // Event
            userContext.UserChanged += this.UserContext_UserChanged;
        }


        // Methods
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // ClaimsPrincipal
            var claimsPrincipal = _userContext.CurrentUser;
            if(claimsPrincipal == null) throw new InvalidOperationException($"{nameof(claimsPrincipal)}=null");

            // Return
            return Task.FromResult(new AuthenticationState(claimsPrincipal));
        }


        // Handlers
        private void UserContext_UserChanged(ClaimsPrincipal claimsPrincipal)
        {
            #region Contracts

            if (claimsPrincipal == null) throw new ArgumentException($"{nameof(claimsPrincipal)}=null");

            #endregion

            // Notify
            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}