namespace Authentication.Local.Events
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Security.Principal;
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
            if (!context.Principal.Identity.IsAuthenticated
                || string.IsNullOrWhiteSpace(context.Principal.Identity.Name))
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

                    UpdateClaims(context.Principal, 
                        userName => !string.Equals(userName, user.UserName),
                        ClaimTypes.NameIdentifier,
                        user.UserName);

                    UpdateClaims(context.Principal,
                        email => !string.Equals(email, user.Email),
                        ClaimTypes.Email,
                        user.Email);

                    UpdateClaims(context.Principal,
                        name => !string.Equals(name, user.FirstName),
                        ClaimTypes.GivenName,
                        user.FirstName);

                    UpdateClaims(context.Principal,
                        surname => !string.Equals(surname, user.Surname),
                        ClaimTypes.Surname,
                        user.Surname);

                    UpdateClaims(context.Principal,
                        dob => !DateTime.Equals(DateTime.Parse(dob), user.DateOfBirth),
                        ClaimTypes.DateOfBirth,
                        user.DateOfBirth.ToString("O"));

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

        private static void UpdateClaims(
            ClaimsPrincipal principal,
            Func<string, bool> condition, 
            string claimType, 
            string claimValue)
        {
            var claim = principal.FindFirst(claimType);
            if (claim != null && condition(claim.Value))
            {
                var identity = principal.Identity as ClaimsIdentity;
                identity?.RemoveClaim(claim);
                identity?.AddClaim(new Claim(claimType, claimValue, claim.ValueType, claim.Issuer));
            }
        }
    }
}