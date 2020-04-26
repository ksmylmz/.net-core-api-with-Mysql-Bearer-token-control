using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Dresses.Repo;
using Microsoft.AspNetCore.Http;

namespace Dresses.Security
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
  
        DataRepo repo;
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder,
            ISystemClock clock           )
            : base(options, logger, encoder, clock)
        {

        }



    protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                   return AuthenticateResult.Fail("Missing Authorization Header");

            string username="";
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                username  = chkToken(authHeader);
            }
            catch(Exception ex)
            {
                
                return AuthenticateResult.Fail("Unexpected error, please contact to system manager");
            }

            if (username == "")
            {
                return AuthenticateResult.Fail(new Exception("Invalid Username Or Password"));
            }
               

            var claims = new[] {
                new Claim(ClaimTypes.NameIdentifier, "1"),
                new Claim(ClaimTypes.Name, username),
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }

        private string chkToken(AuthenticationHeaderValue token)
		{
			repo = new DataRepo();

			return repo.CheckToken(token.Parameter);
		
		}
    }
}