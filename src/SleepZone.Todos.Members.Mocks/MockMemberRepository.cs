using CLK.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Members.Mocks
{
    public class MockMemberRepository : MockRepository<Member, string>, MemberRepository
    {
        // Constructors
        public MockMemberRepository() : base(member => Tuple.Create(member.MemberId))
        {
            // Default

        }


        // Methods

    }
}
