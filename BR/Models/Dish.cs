using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Dish
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<ClientDish> ClientDishes { get; set; }

        public Dish()
        {
            ClientDishes = new HashSet<ClientDish>();
        }
    }
}
