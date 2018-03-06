namespace Authentication.Local.Repositories
{
    using System.Linq;
    using System.Threading.Tasks;
    using Models;
    using Syrx;

    public class UserRepository : IUserRepository
    {
        private readonly ICommander<UserRepository> _commander;

        public UserRepository(ICommander<UserRepository> commander) => 
            _commander = commander;

        public async Task<User> FindByUserName(string username) =>
            (await _commander.QueryAsync<User>(new { username })).FirstOrDefault();
    }
}