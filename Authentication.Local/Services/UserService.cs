namespace Authentication.Local.Services
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Models;

    public class UserService : IUserService
    {
        private readonly IDictionary<string, User> _users;

        public UserService(IDictionary<string, User> users) => _users = users;

        public Task<(bool, User)> ValidateUserCredentialsAsync(string username, string password)
        {
            var isValid = _users.ContainsKey(username) && 
                          string.Equals(_users[username].Password, password, StringComparison.Ordinal);
            var result = (isValid, isValid ? _users[username] : null);
            return Task.FromResult(result);
        }
    }
}