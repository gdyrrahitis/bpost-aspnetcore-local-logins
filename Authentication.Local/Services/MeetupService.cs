namespace Authentication.Local.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Authentication.Local.Repositories;

    public class MeetupService : IMeetupService
    {
        private IMeetupRepository _meetupRepository;

        public MeetupService(IMeetupRepository meetupRepository) => _meetupRepository = meetupRepository;

        public async Task<IEnumerable<Meetup>> FindAllMeetupsAsync() =>
            await _meetupRepository.FindAllMeetupsAsync();

        public async Task<Meetup> FindMeetupByIdAsync(int id) => 
            await _meetupRepository.FindMeetupByIdAsync(id);
    }
}
