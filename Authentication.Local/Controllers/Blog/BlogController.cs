namespace Authentication.Local.Controllers.Blog
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Authentication.Local.Infrastructure.Constants;
    using Authentication.Local.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("blog")]
    public class BlogController : Controller
    {
        private readonly IMemberService _memberService;

        public BlogController(IMemberService memberService) => _memberService = memberService;

        [Route("")]
        public IActionResult Index()
        {
            var vm = new BlogViewModel
            {
                Blogs = new List<string> {
                    "Blog post 1",
                    "Blog post 2",
                    "Blog post 3"
                }
            };

            return View(vm);
        }

        [Route("admin")]
        [Authorize(Policy = Policies.BlogAccessRestriction)]
        public async Task<IActionResult> AdminPanel()
        {
            var members = await _memberService.FindAllMembersAsync();
            var current = await _memberService.FindMemberByUserNameAsync(User.Identity.Name);

            var vm = new AdminPanelViewModel
            {
                Current = current,
                Members = members
            };

            return View(vm);
        }
    }
}