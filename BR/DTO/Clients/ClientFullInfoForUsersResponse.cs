using BR.DTO.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientFullInfoForUsersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string MainImage { get; set; }
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
        public int MaxReserveDays { get; set; }
        public bool IsBarReservation { get; set; }
        public int ReserveDurationAvg { get; set; }
        public int ConfirmationDuration { get; set; }
        public ICollection<string> Photos { get; set; }
        public ICollection<string> SocialLinks { get; set; }
        public ICollection<string> PaymentTypes { get; set; }
        public ICollection<string> ClientTypes { get; set; }
        public ICollection<string> MealTypes { get; set; }
        public ICollection<string> Cuisines { get; set; }
        public ICollection<ClientPhoneInfo> Phones { get; set; }
        public ICollection<EventInfo> Events { get; set; }

    }
}
