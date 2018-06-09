namespace Authentication.Local.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Authentication.Local.Models;
    using Authentication.Local.Repositories;

    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository) => _memberRepository = memberRepository;

        public async Task<IEnumerable<Member>> FindAllMembersAsync() =>
            await _memberRepository.FindAllMembersAsync();

        public async Task<Member> FindMemberByUserNameAsync(string userName) =>
            await _memberRepository.FindMemberByUserNameAsync(userName);
    }
}
