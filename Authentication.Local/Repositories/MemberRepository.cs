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

        public MemberRepository(ICommander<MemberRepository> commander)
        {
            _commander = commander;
        }

        public async Task<IEnumerable<Member>> FindAllMembersAsync(int group) =>
            await _commander.QueryAsync<Member>(new { group });

        public async Task<Member> FindMemberByMeetupAndUserNameAsync(int group, string userName) =>
            (await _commander.QueryAsync<Member>(new { group, userName })).FirstOrDefault();
    }
}
