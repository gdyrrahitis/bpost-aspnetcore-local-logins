namespace Authentication.Local.Repositories
{
    using Authentication.Local.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMeetupRepository
    {
        Task<IEnumerable<Meetup>> FindAllMeetupsAsync();

        Task<Meetup> FindMeetupByIdAsync(int id);
    }
}
