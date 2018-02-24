namespace Authentication.Local.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Syrx;

    public class AttendeeRepository : IAttendeeRepository
    {
        private readonly ICommander<AttendeeRepository> _commander;

        public AttendeeRepository(ICommander<AttendeeRepository> commander)
        {
            _commander = commander;
        }

        public Task<IEnumerable<Attendee>> FindAllAttendeesInMeetupAsync(int group) => 
            _commander.QueryAsync<Attendee>(new { group });

        public async Task<Attendee> FindAttendeeInMeetupAsync(int group, string userName) =>
            (await _commander.QueryAsync<Attendee>(new { group, userName })).FirstOrDefault();
    }
}