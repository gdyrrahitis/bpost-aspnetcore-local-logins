namespace Authentication.Local.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IAttendeeRepository
    {
        Task<IEnumerable<Attendee>> FindAllAttendeesInMeetupAsync(int group);
        Task<Attendee> FindAttendeeInMeetupAsync(int group, string userName);
    }
}