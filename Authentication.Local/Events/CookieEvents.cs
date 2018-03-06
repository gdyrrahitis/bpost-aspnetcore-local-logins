namespace Authentication.Local.Events
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authentication.Cookies;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Models;
    using Repositories;

    public class CookieEvents : CookieAuthenticationEvents
    {
        private readonly IUserRepository _userRepository;
        private readonly ILogger<CookieEvents> _logger;

        public CookieEvents(IUserRepository userRepository, ILogger<CookieEvents> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        public override Task SignedIn(CookieSignedInContext context)
        {
            _logger.LogInformation($"User {context.Principal.Identity.Name} is signed in.");
            return base.SignedIn(context);
        }

        public override Task SigningIn(CookieSigningInContext context)
        {
            context.CookieOptions.SameSite = SameSiteMode.Strict;
            return base.SigningIn(context);
        }

        public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
        {
            if (UserIsNotAuthenticatedOrNameClaimIsEmpty(context))
                return;

            try
            {
                var user = await _userRepository.FindByUserName(context.Principal.Identity.Name);
                if (user != null)
                {
                    var updatedOn = DateTime.Parse(context.Principal.FindFirst("UpdatedOn").Value);
                    if (updatedOn < user.UpdatedOn)
                    {
                        context.RejectPrincipal();
                        return;
                    }

                    UpdateUserClaimsIfChanged(context, user);

                    var claims = context.Principal.Claims;
                    var identity = new ClaimsIdentity(claims, context.Principal.Identity.AuthenticationType);
                    var principal = new ClaimsPrincipal(identity);
                    context.ShouldRenew = true;
                    context.ReplacePrincipal(principal);
                    return;
                }

                context.RejectPrincipal();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{0}");
                context.RejectPrincipal();
            }
        }

        private static void UpdateUserClaimsIfChanged(CookieValidatePrincipalContext context, User user)
        {
            UpdateClaimIfChanged(userName => !string.Equals(userName, user.UserName),
                options =>
                {
                    options.ClaimsPrincipal = context.Principal;
                    options.ClaimType = ClaimTypes.NameIdentifier;
                    options.ClaimValue = user.UserName;
                });

            UpdateClaimIfChanged(email => !string.Equals(email, user.Email),
                options =>
                {
                    options.ClaimsPrincipal = context.Principal;
                    options.ClaimType = ClaimTypes.Email;
                    options.ClaimValue = user.Email;
                });

            UpdateClaimIfChanged(name => !string.Equals(name, user.FirstName),
                options =>
                {
                    options.ClaimsPrincipal = context.Principal;
                    options.ClaimType = ClaimTypes.GivenName;
                    options.ClaimValue = user.FirstName;
                });

            UpdateClaimIfChanged(surname => !string.Equals(surname, user.Surname),
                options =>
                {
                    options.ClaimsPrincipal = context.Principal;
                    options.ClaimType = ClaimTypes.Surname;
                    options.ClaimValue = user.Surname;
                });

            UpdateClaimIfChanged(dob => !DateTime.Equals(DateTime.Parse(dob), user.DateOfBirth),
                options =>
                {
                    options.ClaimsPrincipal = context.Principal;
                    options.ClaimType = ClaimTypes.DateOfBirth;
                    options.ClaimValue = user.DateOfBirth.ToString("O");
                });
        }

        private static bool UserIsNotAuthenticatedOrNameClaimIsEmpty(CookieValidatePrincipalContext context) =>
            !context.Principal.Identity.IsAuthenticated
            || string.IsNullOrWhiteSpace(context.Principal.Identity.Name);

        private static void UpdateClaimIfChanged(
            Func<string, bool> condition,
            Action<Options> optionsAction)
        {
            var options = SetupOptions();
            var claim = options.ClaimsPrincipal.FindFirst(options.ClaimType);
            if (claim != null && condition(claim.Value))
            {
                var identity = options.ClaimsPrincipal.Identity as ClaimsIdentity;
                identity?.RemoveClaim(claim);
                identity?.AddClaim(new Claim(options.ClaimType, options.ClaimValue, claim.ValueType, claim.Issuer));
            }

            Options SetupOptions()
            {
                var opt = new Options();
                optionsAction(opt);
                return opt;
            }
        }

        private class Options
        {
            public ClaimsPrincipal ClaimsPrincipal { get; set; }
            public string ClaimType { get; set; }
            public string ClaimValue { get; set; }
        }
    }
}