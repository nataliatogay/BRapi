using System;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class AdminAccountToken
    {
        [Key]
        public int Id { get; set; }

        public int AdminId { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime Expires { get; set; }

        public virtual Admin Admin { get; set; }
    }
}
