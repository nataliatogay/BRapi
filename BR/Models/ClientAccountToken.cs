using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class ClientAccountToken
    {
        [Key]
        public int Id { get; set; }

        public int ClientId { get; set; }

        [Required]
        public string RefreshToken { get; set; }

        [Required]
        public DateTime Expires { get; set; }

        public virtual Client Client { get; set; }
    }
}
