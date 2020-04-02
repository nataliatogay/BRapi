using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Privilege
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string IdentityRoleId { get; set; }

        [ForeignKey("IdentityRoleId")]
        public virtual IdentityRole IdentityRole { get; set; }

        public virtual ICollection<UserPrivileges> UserPrivileges{ get; set; }

        public Privilege()
        {
            UserPrivileges = new HashSet<UserPrivileges>();
        }
    }
}
