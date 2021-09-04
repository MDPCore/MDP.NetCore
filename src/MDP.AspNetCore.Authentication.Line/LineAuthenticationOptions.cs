using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace MDP.AspNetCore.Authentication.Line
{
    public class LineAuthenticationOptions
    {
        // Properties
        public string ClientId { get; set; }

        public string ClientSecret { get; set; }

        public string SignInScheme { get; set; }
    }
}
