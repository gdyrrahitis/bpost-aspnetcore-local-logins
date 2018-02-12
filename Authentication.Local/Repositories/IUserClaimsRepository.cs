namespace Authentication.Local.Repositories
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IUserClaimsRepository
    {
        Task<IEnumerable<UserClaims>> FindClaimsByUsername(string userName);
    }
}