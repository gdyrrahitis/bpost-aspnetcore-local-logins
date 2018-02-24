namespace Authentication.Local.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Authentication.Local.Repositories;

    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public async Task<IEnumerable<Member>> FindAllMembersAsync(int group) =>
            await _memberRepository.FindAllMembersAsync(group);

        public async Task<Member> FindMemberByMeetupAndUserNameAsync(int group, string userName) =>
            await _memberRepository.FindMemberByMeetupAndUserNameAsync(group, userName);
    }
}
