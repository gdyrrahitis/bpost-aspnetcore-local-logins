namespace Authentication.Local.Infrastructure.Security
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Claims;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class DomainRestrictionRequirement: AuthorizationHandler<DomainRestrictionRequirement>, IAuthorizationRequirement
    {
        private readonly IEnumerable<string> _domains;

        public DomainRestrictionRequirement(IEnumerable<string> domains) => _domains = domains;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            DomainRestrictionRequirement requirement)
        {
            var email = context.User.FindFirst(ClaimTypes.Email);
            if (email == null)
            {
                return Task.CompletedTask;
            }

            var regex = new Regex(@"(?<=@)(?'domain'[^.]+)(?=\.)");
            var domain = regex.Match(email.Value).Groups["domain"];
            if (domain.Success)
            {
                var isPermittedDomain = _domains.Contains(domain.Value);
                if (isPermittedDomain)
                {
                    context.Succeed(requirement);
                }
            }

            return Task.CompletedTask;
        }
    }
}