using System;
using System.Collections.Generic;

namespace EstateNexus.Models.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        public string RoleName { get; set; }
        public string RoleDescription { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        // Navigation Properties
        public virtual ICollection<User> Users { get; set; } = new List<User>();
    }
}
