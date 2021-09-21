using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MDP.Identity
{
    public class Role
    {
        // Properties
        public string RoleId { get; set; }

        public string Name { get; set; }


        // Methods
        public virtual void Verify()
        {
            // Require
            if (string.IsNullOrEmpty(this.RoleId) == true) throw new InvalidOperationException($"{nameof(this.RoleId)}=null");
            if (string.IsNullOrEmpty(this.Name) == true) throw new InvalidOperationException($"{nameof(this.Name)}=null");
        }
    }
}
