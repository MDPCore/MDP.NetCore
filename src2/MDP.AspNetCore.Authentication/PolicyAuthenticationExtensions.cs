using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

namespace MDP.AspNetCore.Authentication
{
    public static class PolicyAuthenticationExtensions
    {
        // Methods
        public static AuthenticationBuilder AddPolicy(this AuthenticationBuilder builder, PolicyAuthenticationSetting? authenticationSetting = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // AddPolicy
            return builder.AddPolicy(PolicyAuthenticationDefaults.AuthenticationScheme, authenticationSetting);
        }

        public static AuthenticationBuilder AddPolicy(this AuthenticationBuilder builder, string authenticationScheme, PolicyAuthenticationSetting? authenticationSetting = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));

            #endregion

            // AuthenticationSetting
            if (authenticationSetting == null) authenticationSetting = new PolicyAuthenticationSetting();

            // PolicyScheme
            var distributePolicyScheme = authenticationScheme;
            var authenticatePolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.AuthenticatePolicyScheme;
            var challengePolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.ChallengePolicyScheme;
            var forbidPolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.ForbidPolicyScheme;
            var signInPolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.SignInPolicyScheme;
            var signOutPolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.SignOutPolicyScheme;

            // DistributePolicyScheme
            builder.AddPolicyScheme(distributePolicyScheme, null, authenticationOptions =>
            {
                // Require
                if (string.IsNullOrEmpty(authenticationSetting.DefaultScheme) == true) throw new InvalidOperationException($"{nameof(authenticationSetting.DefaultScheme)}=null");

                // DefaultScheme
                authenticationOptions.ForwardDefault = authenticationSetting.DefaultScheme;
               
                // ForwardScheme
                if (authenticationSetting.AuthenticatePolicy != null) authenticationOptions.ForwardAuthenticate = authenticatePolicyScheme;
                if (authenticationSetting.ChallengePolicy != null) authenticationOptions.ForwardChallenge = challengePolicyScheme;
                if (authenticationSetting.ForbidPolicy != null) authenticationOptions.ForwardForbid = forbidPolicyScheme;
                if (authenticationSetting.SignInPolicy != null) authenticationOptions.ForwardSignIn = signInPolicyScheme;
                if (authenticationSetting.SignOutPolicy != null) authenticationOptions.ForwardSignOut = signOutPolicyScheme;
            });

            // AuthenticatePolicyScheme
            builder.AddPolicyScheme(authenticatePolicyScheme, null, authenticationOptions =>
            {
                // AuthenticatePolicy
                if (authenticationSetting.AuthenticatePolicy != null)
                {
                    authenticationOptions.ForwardDefaultSelector = context => authenticationSetting.AuthenticatePolicy(context);
                }
            });

            // ChallengePolicyScheme
            builder.AddPolicyScheme(challengePolicyScheme, null, authenticationOptions =>
            {
                // ChallengePolicy
                if (authenticationSetting.ChallengePolicy != null)
                {
                    authenticationOptions.ForwardDefaultSelector = context => authenticationSetting.ChallengePolicy(context);
                }
            });

            // ForbidPolicyScheme
            builder.AddPolicyScheme(forbidPolicyScheme, null, authenticationOptions =>
            {
                // ForbidPolicy        
                if (authenticationSetting.ForbidPolicy != null)
                {
                    authenticationOptions.ForwardDefaultSelector = context => authenticationSetting.ForbidPolicy(context);
                }
            });

            // SignInPolicyScheme
            builder.AddPolicyScheme(signInPolicyScheme, null, authenticationOptions =>
            {
                // SignInPolicy        
                if (authenticationSetting.SignInPolicy != null)
                {
                    authenticationOptions.ForwardDefaultSelector = context => authenticationSetting.SignInPolicy(context);
                }
            });

            // SignOutPolicyScheme
            builder.AddPolicyScheme(signOutPolicyScheme, null, authenticationOptions =>
            {
                // SignOutPolicy        
                if (authenticationSetting.SignOutPolicy != null)
                {
                    authenticationOptions.ForwardDefaultSelector = context => authenticationSetting.SignOutPolicy(context);
                }
            });

            // Return
            return builder;
        }
    }
}
