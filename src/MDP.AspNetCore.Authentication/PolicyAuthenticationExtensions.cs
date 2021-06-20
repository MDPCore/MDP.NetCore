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
        public static AuthenticationBuilder AddPolicy(this AuthenticationBuilder builder, Action<PolicyAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));

            #endregion

            // Return
            return builder.AddPolicy(PolicyAuthenticationDefaults.AuthenticationScheme, configureOptions);
        }

        public static AuthenticationBuilder AddPolicy(this AuthenticationBuilder builder, string authenticationScheme, Action<PolicyAuthenticationOptions> configureOptions = null)
        {
            #region Contracts

            if (builder == null) throw new ArgumentException(nameof(builder));
            if (string.IsNullOrEmpty(authenticationScheme) == true) throw new ArgumentException(nameof(authenticationScheme));
           
            #endregion

            // AuthenticationOptions
            if (configureOptions != null) builder.Services.Configure(authenticationScheme, configureOptions);
            builder.Services.TryAddEnumerable(ServiceDescriptor.Singleton<IPostConfigureOptions<PolicyAuthenticationOptions>, PolicyAuthenticationPostConfigureOptions>());

            // PolicyScheme
            var distributePolicyScheme = authenticationScheme;
            var authenticatePolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.AuthenticatePolicyScheme;
            var challengePolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.ChallengePolicyScheme;
            var forbidPolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.ForbidPolicyScheme;
            var signInPolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.SignInPolicyScheme;
            var signOutPolicyScheme = authenticationScheme + PolicyAuthenticationDefaults.SignOutPolicyScheme;
                       
            // DistributePolicyScheme
            builder.Services.AddOptions<PolicySchemeOptions>(distributePolicyScheme).Configure<IOptionsMonitor<PolicyAuthenticationOptions>>((policyOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // DefaultScheme
                policyOptions.ForwardDefault = authenticationOptions.DefaultScheme;

                // ForwardScheme
                if (authenticationOptions.AuthenticateSchemePolicy != null) policyOptions.ForwardAuthenticate = authenticatePolicyScheme;
                if (authenticationOptions.ChallengeSchemePolicy != null) policyOptions.ForwardChallenge = challengePolicyScheme;
                if (authenticationOptions.ForbidSchemePolicy != null) policyOptions.ForwardForbid = forbidPolicyScheme;
                if (authenticationOptions.SignInSchemePolicy != null) policyOptions.ForwardSignIn = signInPolicyScheme;
                if (authenticationOptions.SignOutSchemePolicy != null) policyOptions.ForwardSignOut = signOutPolicyScheme;
            });
            builder.AddPolicyScheme(distributePolicyScheme, null, null);

            // AuthenticatePolicyScheme
            builder.Services.AddOptions<PolicySchemeOptions>(authenticatePolicyScheme).Configure<IOptionsMonitor<PolicyAuthenticationOptions>>((policyOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // DefaultSelector
                if (authenticationOptions.AuthenticateSchemePolicy != null)
                {
                    policyOptions.ForwardDefaultSelector = context => authenticationOptions.AuthenticateSchemePolicy(context);
                }
            });
            builder.AddPolicyScheme(authenticatePolicyScheme, null, null);

            // ChallengePolicyScheme
            builder.Services.AddOptions<PolicySchemeOptions>(challengePolicyScheme).Configure<IOptionsMonitor<PolicyAuthenticationOptions>>((policyOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // DefaultSelector
                if (authenticationOptions.ChallengeSchemePolicy != null)
                {
                    policyOptions.ForwardDefaultSelector = context => authenticationOptions.ChallengeSchemePolicy(context);
                }
            });
            builder.AddPolicyScheme(challengePolicyScheme, null, null);

            // ForbidPolicyScheme
            builder.Services.AddOptions<PolicySchemeOptions>(forbidPolicyScheme).Configure<IOptionsMonitor<PolicyAuthenticationOptions>>((policyOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // DefaultSelector
                if (authenticationOptions.ForbidSchemePolicy != null)
                {
                    policyOptions.ForwardDefaultSelector = context => authenticationOptions.ForbidSchemePolicy(context);
                }
            });
            builder.AddPolicyScheme(forbidPolicyScheme, null, null);

            // SignInPolicyScheme
            builder.Services.AddOptions<PolicySchemeOptions>(signInPolicyScheme).Configure<IOptionsMonitor<PolicyAuthenticationOptions>>((policyOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // DefaultSelector
                if (authenticationOptions.SignInSchemePolicy != null)
                {
                    policyOptions.ForwardDefaultSelector = context => authenticationOptions.SignInSchemePolicy(context);
                }
            });
            builder.AddPolicyScheme(signInPolicyScheme, null, null);

            // SignOutPolicyScheme
            builder.Services.AddOptions<PolicySchemeOptions>(signOutPolicyScheme).Configure<IOptionsMonitor<PolicyAuthenticationOptions>>((policyOptions, authenticationOptionsMonitor) =>
            {
                // AuthenticationOptions
                var authenticationOptions = authenticationOptionsMonitor.Get(authenticationScheme);
                if (authenticationOptions == null) throw new InvalidOperationException($"{nameof(authenticationOptions)}=null");

                // DefaultSelector
                if (authenticationOptions.SignOutSchemePolicy != null)
                {
                    policyOptions.ForwardDefaultSelector = context => authenticationOptions.SignOutSchemePolicy(context);
                }
            });
            builder.AddPolicyScheme(signOutPolicyScheme, null, null);

            // Return
            return builder;
        }
    }
}
