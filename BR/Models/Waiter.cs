using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Waiter
    {
        public int Id { get; set; }
        [Required]
        public string IdentityId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
       // public DateTime? BirthDate { get; set; }
       // public bool? Gender { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
        [ForeignKey("IdentityId")]
        public virtual IdentityUser Identity { get; set; }
    }
}
