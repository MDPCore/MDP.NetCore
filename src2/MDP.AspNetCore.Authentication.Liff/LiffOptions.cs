using MDP.AspNetCore.Authentication.Line;
using Microsoft.AspNetCore.Http;

namespace MDP.AspNetCore.Authentication.Liff
{
    public class LiffOptions : LineOptions
    {
        // Constructors
        public LiffOptions()
        {
            // Options
            this.CallbackPath = new PathString("/signin-liff");
            this.ClaimsIssuer = LiffDefaults.ClaimsIssuer;
        }


        // Properties
        public string LiffId { get; set; } = String.Empty;

        public string LiffName { get; set; } = String.Empty;

        public string LiffColor { get; set; } = String.Empty;
    }
}
