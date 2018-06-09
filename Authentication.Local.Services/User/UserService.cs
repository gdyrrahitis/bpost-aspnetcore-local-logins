namespace Authentication.Local.Services
{
    using System;
    using System.Threading.Tasks;
    using Models;
    using Repositories;

    public class UserService : IUserService
    {
        private readonly IUserRepository _repository;

        public UserService(IUserRepository repository) => _repository = repository;

        public async Task<(bool, User)> ValidateUserCredentialsAsync(string username, string password)
        {
            var user = await _repository.FindByUserName(username);
            if (user != null && string.Equals(user.Password, password, StringComparison.Ordinal))
            {
                return (true, user);
            }

            return (false, null);
        }

        public async Task<User> FindUserByUserName(string username) => 
            await _repository.FindByUserName(username);
    }
}