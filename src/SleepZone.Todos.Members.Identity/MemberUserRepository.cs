using MDP.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SleepZone.Todos.Members.Identity
{
    public class MemberUserRepository : UserRepository<Member>
    {
        // Fields
        private readonly MemberContext _memberContext = null;


        // Constructors
        public MemberUserRepository(MemberContext memberContext) 
        {
            #region Contracts

            if (memberContext == null) throw new ArgumentException(nameof(memberContext));

            #endregion

            // Default
            _memberContext = memberContext;
        }


        // Methods
        public bool Exists(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Member
            var member = _memberContext.MemberRepository.FindById(userId);
            if (member == null) return false;

            // Return
            return true;
        }

        public bool Exists(Member user)
        {
            #region Contracts

            if (user == null) throw new ArgumentException(nameof(user));

            #endregion

            // Member
            var member = _memberContext.MemberRepository.FindById(user.MemberId);
            if (member == null) return false;

            // Return
            return true;
        }

        public void Add(Member user)
        {
            #region Contracts

            if (user == null) throw new ArgumentException(nameof(user));

            #endregion

            // Add
            _memberContext.MemberRepository.Add(user);
        }

        public Member FindByUserId(string userId)
        {
            #region Contracts

            if (string.IsNullOrEmpty(userId) == true) throw new ArgumentException(nameof(userId));

            #endregion

            // Find
            return _memberContext.MemberRepository.FindById(userId);
        }
    }
}
