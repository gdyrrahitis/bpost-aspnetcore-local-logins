namespace Authentication.Local.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IAttendeeService
    {
        Task<IEnumerable<Attendee>> FindAllAttendeesInMeetupAsync(int group);
        Task<Attendee> FindAttendeeInMeetupAsync(int group, string userName);
    }
}