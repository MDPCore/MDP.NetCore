using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Members
{
    public class MemberContext
    {
        // Fields
        private readonly MemberRepository _memberRepository = null;


        // Constructors
        public MemberContext(MemberRepository memberRepository)
        {
            #region Contracts

            if (memberRepository == null) throw new ArgumentException(nameof(memberRepository));
           
            #endregion

            // Default
            _memberRepository = memberRepository;
        }


        // Properties
        public MemberRepository MemberRepository { get { return _memberRepository; } }
    }
}
