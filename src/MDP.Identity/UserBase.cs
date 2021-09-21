using System;

namespace MDP.Identity
{
    public class UserBase
    {
        // Properties
        public string UserId { get; set; }

        public string Name { get; set; }


        // Methods
        public virtual void Verify()
        {
            // Require
            if (string.IsNullOrEmpty(this.UserId) == true) throw new InvalidOperationException($"{nameof(this.UserId)}=null");
            if (string.IsNullOrEmpty(this.Name) == true) throw new InvalidOperationException($"{nameof(this.Name)}=null");
        }
    }
}