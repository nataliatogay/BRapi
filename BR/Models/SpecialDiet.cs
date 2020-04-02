using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class SpecialDiet
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<ClientSpecialDiet> ClientSpecialDiets{ get; set; }

        public SpecialDiet()
        {
            ClientSpecialDiets = new HashSet<ClientSpecialDiet>();
        }
    }
}
