namespace Authentication.Local.Infrastructure.Security
{
    using System.Threading.Tasks;
    using Authentication.Local.Infrastructure.Constants;
    using Microsoft.AspNetCore.Authorization;

    public class MeetupFounderHandler : AuthorizationHandler<MeetupAccessRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MeetupAccessRequirement requirement)
        {
            if (context.User.IsInRole(Roles.Founder))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
