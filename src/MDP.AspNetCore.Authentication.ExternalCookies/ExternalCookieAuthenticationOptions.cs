using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.ExternalCookies
{
    public class ExternalCookieAuthenticationOptions
    {
        // Constructors
        public ExternalCookieAuthenticationOptions()
        {
            // Options
            this.CallbackPath = new PathString("/signin-external");
        }


        // Properties
        public string DefaultScheme { get; set; }

        public PathString CallbackPath { get; set; }
    }
}
