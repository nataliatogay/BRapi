using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class MealType
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<ClientMealType> ClientMealTypes { get; set; }

        public MealType()
        {
            ClientMealTypes = new HashSet<ClientMealType>();
        }
    }
}
