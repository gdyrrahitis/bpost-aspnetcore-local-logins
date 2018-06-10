namespace Authentication.Local.Infrastructure.Security
{
    using System;
    using System.Threading.Tasks;
    using Authentication.Local.Services;
    using Microsoft.AspNetCore.Authorization;

    public class BlogFreezeHandler : AuthorizationHandler<BlogAccessRequirement>
    {
        private readonly IMemberService _memberService;

        public BlogFreezeHandler(IMemberService memberService) => _memberService = memberService;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            BlogAccessRequirement requirement)
        {
            try
            {
                var member = await _memberService.FindMemberByUserNameAsync(context.User.Identity.Name);
                if (member == null)
                {
                    context.Fail();
                }

                if (member.IsFrozen)
                {
                    context.Fail();
                }
            }
            catch (Exception)
            {
                context.Fail();
            }
        }
    }
}
