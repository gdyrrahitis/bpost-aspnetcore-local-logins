namespace Authentication.Local.Infrastructure.Security
{
    using System;
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Authentication.Local.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class MeetupRsvpHandler : AuthorizationHandler<MeetupAccessRequirement>
    {
        private readonly IAttendeeService _attendeeService;

        public MeetupRsvpHandler(IAttendeeService attendeeService) => _attendeeService = attendeeService;

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context,
            MeetupAccessRequirement requirement)
        {
            try
            {
                if (context.Resource is AuthorizationFilterContext mvcContext)
                {
                    var id = Convert.ToInt32(mvcContext.RouteData.Values["id"]);
                    var rsvp = await _attendeeService
                        .FindAttendeeInMeetupAsync(id, context.User.Identity.Name);

                    if (HasRsvped(rsvp))
                    {
                        context.Succeed(requirement);
                        return;
                    }
                }

                context.Fail();
            }
            catch(Exception ex)
            {
                context.Fail();
            }
        }

        private static bool HasRsvped(Attendee rsvp) => rsvp != null;
    }
}
