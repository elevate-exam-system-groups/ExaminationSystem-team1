using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace ExaminationSystem.Features.Authentication
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

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.TryGetValue("Authorization", out var authorizationHeaderValues))
            {
                return AuthenticateResult.NoResult();
            }

            var authorizationHeader = authorizationHeaderValues.ToString();
            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return AuthenticateResult.NoResult();
            }

            try
            {
                // Parse the basic authentication credentials (format: Basic Base64String)
                var authHeaderValue = AuthenticationHeaderValue.Parse(authorizationHeader);
                var credentialBytes = Convert.FromBase64String(authHeaderValue.Parameter ?? string.Empty);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);

                if (credentials.Length != 2)
                {
                    return AuthenticateResult.Fail("Invalid Authorization Header Format");
                }

                var username = credentials[0];
                var password = credentials[1];

                // Resolve DB Context directly from request services
                var dbContext = Context.RequestServices.GetRequiredService<Domain.Data.Context>();
                if (!(username == "amr" && password == "Admin"))
                {
                    return AuthenticateResult.Fail("Invalid Credentials");
                }


                // Find user in database by Username or Email
                //var user = await dbContext.Users
                //    .FirstOrDefaultAsync(u => u.Email == username || u.UserName == username);

                //if (user == null)
                //{
                //    return AuthenticateResult.Fail("Invalid Credentials");
                //}

                //// Verify password using BCrypt
                //bool isPasswordValid = BCrypt.Net.BCrypt.Verify(password, user.PasswordHash);
                //if (!isPasswordValid)
                //{
                //    return AuthenticateResult.Fail("Invalid Credentials");
                //}

                //// Determine user role dynamically by checking existence in Admins or Students tables
                //string role = "Student"; // Default role
                //bool isAdmin = await dbContext.Admins.AnyAsync(a => a.UserId == user.Id);
                //if (isAdmin)
                //{
                //    role = "Admin";
                //}

                // Explicitly build the .NET Identity object hierarchy to show how it works under the hood
                
                // 1. Create a collection of Claim objects
                var claims = new[] {
                    new Claim(ClaimTypes.NameIdentifier,"1"),
                    new Claim(ClaimTypes.Name, username),
                    new Claim (ClaimTypes.Email, "amr@gmail.com"),
                    new Claim(ClaimTypes.Role, "Admin")
                };

                // 2. Instantiate a ClaimsIdentity passing the claims and naming the authentication type
                var identity = new ClaimsIdentity(claims, Scheme.Name);

                // 3. Instantiate a ClaimsPrincipal wrapping that identity
                var principal = new ClaimsPrincipal(identity);

                // 4. Construct an AuthenticationTicket using the principal and scheme name
                var ticket = new AuthenticationTicket(principal, Scheme.Name);

                // 5. Return success with the built ticket
                return AuthenticateResult.Success(ticket);
            }
            catch (Exception ex)
            {
                return AuthenticateResult.Fail($"Authorization Header Parsing Failed: {ex.Message}");
            }
        }
    }
}
