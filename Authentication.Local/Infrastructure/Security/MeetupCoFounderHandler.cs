namespace Authentication.Local.Infrastructure.Security
{
    using System.Threading.Tasks;
    using Authentication.Local.Infrastructure.Constants;
    using Microsoft.AspNetCore.Authorization;

    public class MeetupCoFounderHandler : AuthorizationHandler<MeetupAccessRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MeetupAccessRequirement requirement)
        {
            if (context.User.IsInRole(Roles.CoFounder))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
