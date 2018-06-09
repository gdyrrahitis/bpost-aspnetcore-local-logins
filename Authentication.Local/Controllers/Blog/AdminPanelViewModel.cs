namespace Authentication.Local.Controllers.Blog
{
    using Authentication.Local.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class AdminPanelViewModel
    {
        public IEnumerable<Member> Members { get; set; }
        public Member Current { get; set; }
    }
}
