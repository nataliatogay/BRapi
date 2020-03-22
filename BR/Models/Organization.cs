using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Organization
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string LogoPath { get; set; }

        public virtual ICollection<Owner> Owners{ get; set; }

        public Organization()
        {
            Owners = new HashSet<Owner>();
        }
    }
}
