namespace Authentication.Local.Services
{
    using Authentication.Local.Models;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IMemberService
    {
        Task<Member> FindMemberByUserNameAsync(string userName);
        Task<IEnumerable<Member>> FindAllMembersAsync();
    }
}
