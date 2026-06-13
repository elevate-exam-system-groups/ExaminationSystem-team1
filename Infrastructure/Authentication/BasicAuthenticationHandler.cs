using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ExaminationSystem.Infrastructure.Authentication
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public BasicAuthenticationHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            var authorizationHeader = authorizationHeaderValues.ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return Task.FromResult(AuthenticateResult.NoResult());
            }

            try
            {
                var authHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);
                var credentialBytes = Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                
                if (credentials.Length != 2)
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Authorization Header Format"));
                }

                var username = credentials[0];
                var password = credentials[1];

                string userId;
                string role;

                if (username == "admin" && password == "password")
                {
                    userId = "admin-id";
                    role = "Admin";
                }
                else if (username == "student" && password == "password")
                {
                    userId = "a0000001-0000-0000-0000-000000000001";
                    role = "Student";
                }
                else
                {
                    return Task.FromResult(AuthenticateResult.Fail("Invalid Credentials"));
                }

                // 1. Create a collection of Claim objects
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, username),
                    new Claim(ClaimTypes.Role, role)
                };

                // 2. Instantiate a ClaimsIdentity passing the claims and naming the authentication type
                var identity = new ClaimsIdentity(claims, Scheme.Name);

                // 3. Instantiate a ClaimsPrincipal wrapping that identity
                var principal = new ClaimsPrincipal(identity);

                // 4. Construct an AuthenticationTicket using the principal and scheme name
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                // 5. Return success
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }
            catch (Exception ex)
            {
                return Task.FromResult(AuthenticateResult.Fail($"Authorization Header Parsing Failed: {ex.Message}"));
            }
        }
    }
}
