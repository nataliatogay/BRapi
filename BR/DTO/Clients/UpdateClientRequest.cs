using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class UpdateClientRequest
    {
        public int ClientId { get; set; }

        [Required]
        public string RestaurantName { get; set; }

        [Required]
        public string AdminName { get; set; }

        [Required]
        public string AdminPhoneNumber { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public int OpenTime { get; set; }

        public int CloseTime { get; set; }

        public string Description { get; set; }

        public int MaxReserveDays { get; set; }

        public int ReserveDurationAvg { get; set; }

        public int? BarReserveDurationAvg { get; set; }

        public int ConfirmationDuration { get; set; }

        public int PriceCategory { get; set; }

        public ICollection<string> SocialLinks { get; set; }

        public ICollection<int> ClientTypeIds { get; set; }

        public ICollection<int> MealTypeIds { get; set; }

        public ICollection<int> CuisineIds { get; set; }

        public ICollection<int> DishIds { get; set; }

        public ICollection<int> GoodForIds { get; set; }

        public ICollection<int> SpecialDietIds { get; set; }

        public ICollection<int> FeatureIds { get; set; }

        public ICollection<ClientPhoneInfo> Phones { get; set; }
    }
}
