using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class Cuisine
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }
        public virtual ICollection<ClientCuisine> ClientCuisines { get; set; }

        public Cuisine()
        {
            ClientCuisines = new HashSet<ClientCuisine>();
        }
    }
}
