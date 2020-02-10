using BR.DTO.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Requests
{
    public class NewRequestRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Address { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }

        public int OpenTime { get; set; }

        public int CloseTime { get; set; }

        public bool IsParking { get; set; }

        public bool IsWiFi { get; set; }

        public bool? IsLiveMusic { get; set; }

        public bool IsOpenSpace { get; set; }

        public bool IsChildrenZone { get; set; }

        public bool IsBusinessLunch { get; set; }

        public string AdditionalInfo { get; set; }
        [Required]
        public string MainImage { get; set; }

        public int MaxReserveDays { get; set; }

        public ICollection<string> SocialLinks { get; set; }

        public ICollection<int> PaymentTypeIds { get; set; }
        public ICollection<int> MealTypeIds { get; set; }

        public ICollection<int> ClientTypeIds { get; set; }

        public ICollection<int> CuisineIds { get; set; }

        public ICollection<ClientPhoneInfo> Phones { get; set; }

    }

   
}
