namespace Authentication.Local.Repositories
{
    using Authentication.Local.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMemberRepository
    {
        Task<Member> FindMemberByUserNameAsync(string userName);
        Task<IEnumerable<Member>> FindAllMembersAsync();
    }
}
