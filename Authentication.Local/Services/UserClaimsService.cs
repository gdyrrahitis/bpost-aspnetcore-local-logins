namespace Authentication.Local.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;
    using Repositories;

    public class UserClaimsService: IUserClaimsService
    {
        private readonly IUserClaimsRepository _repository;

        public UserClaimsService(IUserClaimsRepository repository) => _repository = repository;

        public async Task<IEnumerable<UserClaims>> FindUserClaimsByUserNameAsync(string userName) => 
            await _repository.FindClaimsByUserNameAsync(userName);
    }
}