using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class AccountToken
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string IdentityUserId { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime Expires { get; set; }

        [Required]
        public string NotificationTag { get; set; }

        public virtual IdentityUser IdentityUser { get; set; }
    }
}
