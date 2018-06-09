namespace Authentication.Local.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Syrx;

    public class UserClaimsRepository: IUserClaimsRepository
    {
        private readonly ICommander<UserClaimsRepository> _commander;

        public UserClaimsRepository(ICommander<UserClaimsRepository> commander) => _commander = commander;

        public async Task<IEnumerable<UserClaims>> FindClaimsByUserId(int id) =>
            await _commander.QueryAsync<UserClaims>(new { userId = id });
    }
}