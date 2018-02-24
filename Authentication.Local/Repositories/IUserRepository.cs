namespace Authentication.Local.Repositories
{
    using System.Threading.Tasks;
    using Models;

    public interface IUserRepository
    {
        Task<User> FindByUserNameAsync(string username);
    }
}