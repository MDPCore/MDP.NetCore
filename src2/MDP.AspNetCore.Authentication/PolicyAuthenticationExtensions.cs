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
            if (string.IsNullOrEmpty(authenticationSetting.DefaultScheme) == true) throw new InvalidOperationException($"{nameof(authenticationSetting.DefaultScheme)}=null");

            // PolicyScheme
            builder.AddPolicyScheme(authenticationScheme, null, authenticationOptions =>
            {
                // ForwardDefault
                authenticationOptions.ForwardDefault = authenticationSetting.DefaultScheme;

                // ForwardDefaultSelector
                authenticationOptions.ForwardDefaultSelector = context =>
                {
                    // PolicySchemeSelectorList
                    var policySchemeSelectorList = context.RequestServices.GetRequiredService<IList<PolicySchemeSelector>>();
                    if (policySchemeSelectorList == null) throw new InvalidOperationException($"{nameof(policySchemeSelectorList)}=null");

                    // PolicySchemeSelector
                    foreach (var policySchemeSelector in policySchemeSelectorList)
                    {
                        // Check
                        if (policySchemeSelector.Check(context) == false) continue;

                        // Apply
                        return policySchemeSelector.AuthenticationScheme;
                    }

                    // DefaultScheme
                    return authenticationSetting.DefaultScheme;
                };
            });

            // Return
            return builder;
        }
    }
}
