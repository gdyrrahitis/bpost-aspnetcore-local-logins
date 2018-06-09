namespace Authentication.Local.Infrastructure.Security.Handlers
{
    using Microsoft.AspNetCore.Authorization;
    using System.Threading.Tasks;

    public class BlogAnonymousHandler : AuthorizationHandler<BlogAccessRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            BlogAccessRequirement requirement)
        {
            if (!context.User.Identity.IsAuthenticated)
            {
                context.Fail();
            }

            return Task.CompletedTask;
        }
    }
}
