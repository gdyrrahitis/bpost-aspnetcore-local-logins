namespace Authentication.Local.Infrastructure.Security
{
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.Extensions.Options;

    public class BlogAdminHandler : AuthorizationHandler<BlogAccessRequirement>
    {
        private readonly Roles _roles;

        public BlogAdminHandler(IOptions<Roles> options) => _roles = options.Value;

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            BlogAccessRequirement requirement)
        {
            if (context.User.IsInRole(_roles.Admin))
            {
                context.Succeed(requirement);
            }

            return Task.CompletedTask;
        }
    }
}
