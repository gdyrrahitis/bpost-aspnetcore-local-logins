namespace Authentication.Local.Controllers.Home
{
    using Authentication.Local.Models;
    using Infrastructure.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;

    public class HomeController : Controller
    {
        private readonly AuthSettings _settings;

        public HomeController(IOptions<AuthSettings> options) => _settings = options.Value;

        public IActionResult Index() => View();

        [Authorize]
        [Route("profile")]
        public IActionResult Profile()
        {
            var model = new ProfileViewModel
            {
                Name = User.Identity.Name,
                Claims = User.Claims
            };
            return View(model);
        }

        [Authorize(Policy = Policies.AgeRestriction)]
        [Route("protected")]
        public IActionResult Protected()
        {
            ViewBag.AgeRestriction = _settings.Age;
            return View();
        }

        [Authorize(Policy = Policies.DomainRestriction)]
        [Route("domain")]
        public IActionResult Domain()
        {
            ViewBag.DomainRestriction = _settings.Domains;
            return View();
        }

        [Route("denied")]
        public IActionResult Denied() => View();
    }
}
