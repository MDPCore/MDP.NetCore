using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Members
{
    public interface MemberRepository
    {
        // Methods
        void Add(Member member);

        void Update(Member member);

        void Remove(string memberId);

        Member FindById(string memberId);

        List<Member> FindAll();
    }
}
