namespace Authentication.Local.Controllers.Meetup
{
    using Authentication.Local.Models;
    using System.Collections.Generic;

    public class MeetupViewModel
    {
        public string Meetup { get; set; }
        public IEnumerable<Attendee> Attendees { get; set; }
    }
}
