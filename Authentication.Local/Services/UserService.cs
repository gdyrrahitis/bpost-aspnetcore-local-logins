namespace Authentication.Local.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public class UserService : IUserService
    {
        private readonly IDictionary<string, User> _users;

        public UserService(IDictionary<string, User> users) => _users = users;

        public async Task<(bool, User)> ValidateUserCredentialsAsync(string username, string password)
        {
            var isValid = await Task.FromResult(_users.ContainsKey(username) &&
                string.Equals(_users[username].Password, password));

            return (isValid, isValid ? _users[username] : null);
        }
    }
}