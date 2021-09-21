using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class UserRole
    {
        // Properties
        public string UserId { get; set; }

        public string RoleId { get; set; }


        // Methods
        public virtual void Verify()
        {
            // Require
            if (string.IsNullOrEmpty(this.UserId) == true) throw new InvalidOperationException($"{nameof(this.UserId)}=null");
            if (string.IsNullOrEmpty(this.RoleId) == true) throw new InvalidOperationException($"{nameof(this.RoleId)}=null");
        }
    }
}