namespace Authentication.Local.Repositories
{
    using System.Threading.Tasks;
    using Models;

    public interface IUserRepository
    {
        Task<User> FindByUserName(string username);
    }
}