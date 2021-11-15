using MDP.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BiosWorkshop.CRM.Members
{
    public class MemberUser : BaseUser
    {
        // Properties
        public string Gender { get; set; }

        public string Phone { get; set; }

        public string Mail { get; set; }

        public string Address { get; set; }
    }
}
