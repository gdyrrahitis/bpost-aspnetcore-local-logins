namespace Authentication.Local.Models
{
    using System.Collections.Generic;

    public class AuthSettings
    {
        public int Age { get; set; }
        public IEnumerable<string> Domains { get; set; }
    }
}
