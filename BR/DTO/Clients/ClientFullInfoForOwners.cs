﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientFullInfoForOwners
    {
        public int Id { get; set; }

        public string ClientName { get; set; }

        public string OrganizationName { get; set; }

        public int OrganizationId { get; set; }

        public string Email { get; set; }

        public string AdminName { get; set; }

        public string AdminPhoneNumber { get; set; }

        public string LogoPath { get; set; }

        public ICollection<ClientImageInfo> Images { get; set; }

        public DateTime RegistrationDate { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public int OpenTime { get; set; }

        public int CloseTime { get; set; }

        public int PriceCategory { get; set; }

        public DateTime? Confirmed { get; set; }

        public DateTime? Blocked { get; set; }

        public DateTime? Deleted { get; set; }

        public int MaxReserveDays { get; set; }

        public int ReserveDurationAvg { get; set; }

        public int? BarReserveDurationAvg { get; set; }

        public int ConfirmationDuration { get; set; }

        public string Description { get; set; }

        public ICollection<string> SocialLinks { get; set; }

        public ICollection<ClientPhoneInfo> Phones { get; set; }

        public ICollection<int> MealTypeIds { get; set; }

        public ICollection<int> ClientTypeIds { get; set; }

        public ICollection<int> CuisineIds { get; set; }

        public ICollection<int> SpecialDietIds { get; set; }

        public ICollection<int> GoodForIds { get; set; }

        public ICollection<int> DishIds { get; set; }

        public ICollection<int> FeatureIds { get; set; }
    }
}
