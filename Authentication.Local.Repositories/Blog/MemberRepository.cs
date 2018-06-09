namespace Authentication.Local.Repositories
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Syrx;

    public class MemberRepository : IMemberRepository
    {
        private readonly ICommander<MemberRepository> _commander;

        public MemberRepository(ICommander<MemberRepository> commander) => _commander = commander;

        public async Task<IEnumerable<Member>> FindAllMembersAsync() =>
            await _commander.QueryAsync<Member>();

        public async Task<Member> FindMemberByUserNameAsync(string userName) =>
            (await _commander.QueryAsync<Member>(new { userName })).FirstOrDefault();
    }
}
