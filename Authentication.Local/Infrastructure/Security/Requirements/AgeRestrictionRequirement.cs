namespace Authentication.Local.Infrastructure.Security
{
    using System;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;

    public class AgeRestrictionRequirement: AuthorizationHandler<AgeRestrictionRequirement>, IAuthorizationRequirement
    {
        private readonly int _minimumAge;

        public AgeRestrictionRequirement(int minimumAge) => _minimumAge = minimumAge;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, 
            AgeRestrictionRequirement requirement)
        {
            var dateOfBirth = context.User.FindFirst(ClaimTypes.DateOfBirth);
            if (dateOfBirth == null)
            {
                return Task.CompletedTask;
            }

            var isEligible = DateTime.UtcNow.AddYears(-_minimumAge) >= DateTime.Parse(dateOfBirth.Value);
            if (isEligible)
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}