using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace MDP.AspNetCore.Authentication.Liff
{
    public static class LiffAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddLiffAuthentication(this IServiceCollection services, LiffAuthenticationSetting? authenticationSetting = null)
        {
            #region Contracts

            if (services == null) throw new ArgumentException($"{nameof(services)}=null");
         
            #endregion

            // AuthenticationSetting
            if (authenticationSetting == null) authenticationSetting = new LiffAuthenticationSetting();
            if (string.IsNullOrEmpty(authenticationSetting.LiffId) == true) throw new InvalidOperationException($"{nameof(authenticationSetting.LiffId)}=null");
            if (string.IsNullOrEmpty(authenticationSetting.LiffName) == true) throw new InvalidOperationException($"{nameof(authenticationSetting.LiffName)}=null");
            if (string.IsNullOrEmpty(authenticationSetting.LiffColor) == true) throw new InvalidOperationException($"{nameof(authenticationSetting.LiffColor)}=null");
            if (string.IsNullOrEmpty(authenticationSetting.ClientId) == true) throw new InvalidOperationException($"{nameof(authenticationSetting.ClientId)}=null");
            if (string.IsNullOrEmpty(authenticationSetting.ClientSecret) == true) throw new InvalidOperationException($"{nameof(authenticationSetting.ClientSecret)}=null");

            // AuthenticationBuilder   
            var authenticationBuilder = services.AddAuthentication();

            // Liff
            authenticationBuilder.AddLiff(options =>
            {
                // Options
                options.LiffId = authenticationSetting.LiffId;
                options.LiffName = authenticationSetting.LiffName;
                options.LiffColor = authenticationSetting.LiffColor;
                options.ClientId = authenticationSetting.ClientId;
                options.ClientSecret = authenticationSetting.ClientSecret;

                // SignIn
                options.SignInPath("/SignIn");
                options.SignInScheme = ExternalCookieAuthenticationDefaults.AuthenticationScheme;
            });

            // Return
            return authenticationBuilder;
        }
    }
}
