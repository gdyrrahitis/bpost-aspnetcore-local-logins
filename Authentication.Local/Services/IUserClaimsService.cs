namespace Authentication.Local.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public interface IUserClaimsService
    {
        Task<IEnumerable<UserClaims>> FindUserClaimsByUsername(string userName);
    }
}