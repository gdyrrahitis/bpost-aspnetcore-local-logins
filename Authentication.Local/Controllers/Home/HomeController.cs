namespace Authentication.Local.Controllers.Home
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using Infrastructure.Constants;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Configuration;
    using Models;

    public class HomeController : Controller
    {
        private readonly IConfiguration _configuration;

        public HomeController(IConfiguration configuration) => _configuration = configuration;

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
            ViewBag.AgeRestriction = _configuration.GetSection("Policies:Age").Get<int>();
            return View();
        }

        [Authorize(Policy = Policies.DomainRestriction)]
        [Route("domain")]
        public IActionResult Domain()
        {
            ViewBag.DomainRestriction = _configuration.GetSection("Policies:Domains")
                .Get<List<string>>();
            return View();
        }

        [Route("denied")]
        public IActionResult Denied() => View();

        public IActionResult Error() =>
            View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
