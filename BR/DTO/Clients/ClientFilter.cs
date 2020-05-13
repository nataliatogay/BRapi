using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientFilter
    {
        public string Name { get; set; }

        public CoordinatesFilter CoordFilter { get; set; }

        public ICollection<int> MealTypeIds { get; set; }

        public ICollection<int> CuisineIds { get; set; }

        public ICollection<int> ClientTypeIds { get; set; }

        public ICollection<int> FeatureIds { get; set; }

        public ICollection<int> SpecialDietIds { get; set; }

        public ICollection<int> DishIds { get; set; }

        public ICollection<int> GoodForIds { get; set; }

        public ICollection<int> PriceCategories { get; set; }

        public bool? IsFavourite { get; set; }
    }

    public class CoordinatesFilter
    {
        [Required]
        public double Lat { get; set; }

        [Required]
        public double Long { get; set; }

        [Required]
        public double Radius { get; set; }
    }
}
