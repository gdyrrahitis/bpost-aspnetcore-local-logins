namespace Authentication.Local.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repositories;

    public class AttendeeService: IAttendeeService
    {
        private readonly IAttendeeRepository _attendeeRepository;

        public AttendeeService(IAttendeeRepository attendeeRepository)
        {
            _attendeeRepository = attendeeRepository;
        }

        public async Task<IEnumerable<Attendee>> FindAllAttendeesInMeetupAsync(int group) => 
            await _attendeeRepository.FindAllAttendeesInMeetupAsync(@group);

        public async Task<Attendee> FindAttendeeInMeetupAsync(int group, string userName) =>
            await _attendeeRepository.FindAttendeeInMeetupAsync(group, userName);
    }
}