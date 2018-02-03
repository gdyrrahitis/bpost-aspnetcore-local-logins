namespace Authentication.Local.Services
{
    using System.Threading.Tasks;
    using Models;

    public interface IUserService
    {
        Task<(bool, User)> ValidateUserCredentialsAsync(string username, string password);
    }
}