using System.Security.Claims;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace OEBG_PLAGG_POC.Security {
    public class DevAuthenticationHandler(
        IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        IConfiguration configuration) : AuthenticationHandler<AuthenticationSchemeOptions>(options, logger, encoder) {

        protected override Task<AuthenticateResult> HandleAuthenticateAsync() {
            var userName = configuration["Authentication:DevUserName"];
            if (string.IsNullOrWhiteSpace(userName)) {
                userName = $"DEV\\{Environment.UserName}";
            }

            var claims = new[] {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.NameIdentifier, userName)
            };

            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
