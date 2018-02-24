namespace Authentication.Local.Services
{
    using Authentication.Local.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMeetupService
    {
        Task<IEnumerable<Meetup>> FindAllMeetupsAsync();

        Task<Meetup> FindMeetupByIdAsync(int id);
    }
}
