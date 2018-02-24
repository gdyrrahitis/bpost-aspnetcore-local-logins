namespace Authentication.Local.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Syrx;

    public class MeetupRepository : IMeetupRepository
    {
        private ICommander<MeetupRepository> _commander;

        public MeetupRepository(ICommander<MeetupRepository> commander)
        {
            _commander = commander;
        }

        public async Task<IEnumerable<Meetup>> FindAllMeetupsAsync() =>
            await _commander.QueryAsync<Meetup>();

        public async Task<Meetup> FindMeetupByIdAsync(int id) =>
            (await _commander.QueryAsync<Meetup>(new { id })).FirstOrDefault();
    }
}
