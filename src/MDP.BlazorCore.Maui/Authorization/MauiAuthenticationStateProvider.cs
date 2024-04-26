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
        private UserManager _userManager;


        // Constructors
        public MauiAuthenticationStateProvider(UserManager userManager)
        {
            #region Contracts

            if (userManager == null) throw new ArgumentException($"{nameof(userManager)}=null");

            #endregion

            // Default
            _userManager = userManager;

            // Event
            userManager.UserChanged += this.UserManager_UserChanged;
        }


        // Methods
        public override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            // ClaimsPrincipal
            var claimsPrincipal = _userManager.CurrentUser;
            if(claimsPrincipal == null) throw new InvalidOperationException($"{nameof(claimsPrincipal)}=null");

            // Return
            return Task.FromResult(new AuthenticationState(claimsPrincipal));
        }


        // Handlers
        private void UserManager_UserChanged(ClaimsPrincipal claimsPrincipal)
        {
            #region Contracts

            if (claimsPrincipal == null) throw new ArgumentException($"{nameof(claimsPrincipal)}=null");

            #endregion

            // Notify
            this.NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
        }
    }
}