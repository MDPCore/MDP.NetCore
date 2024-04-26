using MDP.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MDP.BlazorCore.Maui
{
    [Service<UserManager>(singleton:true)]
    public class UserManager
    {
        // Properties
        public ClaimsPrincipal CurrentUser { get; private set; } = new ClaimsPrincipal();


        // Methods
        public Task SignInAsync(ClaimsPrincipal user)
        {
            #region Contracts

            if (user == null) throw new ArgumentException($"{nameof(user)}=null");

            #endregion

            // CurrentUser
            var currentUser = user;
            this.CurrentUser = currentUser;

            // Raise
            this.OnUserChanged(currentUser);

            // Return
            return Task.CompletedTask;
        }

        public Task SignOutAsync() 
        {
            // CurrentUser
            var currentUser = new ClaimsPrincipal();
            this.CurrentUser = currentUser;

            // Raise
            this.OnUserChanged(currentUser);

            // Return
            return Task.CompletedTask;
        }


        // Events
        public event Action<ClaimsPrincipal> UserChanged;
        protected void OnUserChanged(ClaimsPrincipal user)
        {
            #region Contracts

            if (user == null) throw new ArgumentException($"{nameof(user)}=null");

            #endregion

            // Raise
            var handler = this.UserChanged;
            if (handler != null)
            {
                handler(user);
            }
        }
    }
}
