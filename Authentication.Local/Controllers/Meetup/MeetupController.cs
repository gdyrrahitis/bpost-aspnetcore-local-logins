namespace Authentication.Local.Controllers.Meetup
{
    using System.Linq;
    using System.Threading.Tasks;
    using Authentication.Local.Infrastructure.Constants;
    using Authentication.Local.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Route("meetup")]
    public class MeetupController : Controller
    {
        private readonly IAttendeeService _attendeeService;
        private readonly IMeetupService _meetupService;

        public MeetupController(IAttendeeService attendeeService,
            IMeetupService meetupService)
        {
            _attendeeService = attendeeService;
            _meetupService = meetupService;
        }

        [Route("list")]
        public async Task<IActionResult> Meetups()
        {
            var meetups = await _meetupService.FindAllMeetupsAsync();
            return View(meetups);
        }

        [Authorize(Policy = Policies.MeetupRestriction)]
        [Route("{id}")]
        public async Task<IActionResult> Meetup(int id)
        {
            var meetup = await _meetupService.FindMeetupByIdAsync(id);
            var attendees = await _attendeeService.FindAllAttendeesInMeetupAsync(id);

            var vm = new MeetupViewModel
            {
                Attendees = attendees,
                Meetup = meetup.Name
            };

            return View(vm);
        }
    }
}