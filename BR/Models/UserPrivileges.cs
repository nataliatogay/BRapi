using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class UserPrivileges
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public string IdentityId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int PrivilegeId { get; set; }

        [ForeignKey("IdentityId")]
        public virtual IdentityUser IdentityUser { get; set; }

        [ForeignKey("PrivilegeId")]
        public virtual Privilege Privilege { get; set; }
    }
}
