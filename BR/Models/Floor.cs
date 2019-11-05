using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class Floor
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        public int ClientId { get; set; }

        public virtual Client Client { get; set; }

        public virtual ICollection<Hall> Halls { get; set; }

        public Floor()
        {
            Halls = new HashSet<Hall>();
        }
    }
}
