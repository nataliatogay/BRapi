using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class ClientInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public float Lat { get; set; }
        public float Long { get; set; }
        public int OpenTime { get; set; }
        public int CloseTime { get; set; }
        public bool IsPasking { get; set; }
        public bool IsWiFi { get; set; }
        public bool? IsLiveMusic { get; set; }
        public bool IsOpenSpace { get; set; }
        public bool IsChildrenZone { get; set; }
        public bool IsBusinessLunch { get; set; }
        public string AdditionalInfo { get; set; }
        public string MainImage { get; set; }
        public int MaxReserveDays { get; set; }
        public string Email { get; set; }
        public ICollection<string> SocialLinks { get; set; }
        public ICollection<string> PaymentTypes { get; set; } 
        public ICollection<string> ClientTypes { get; set; }
        public ICollection<string> MealTypes { get; set; }
        public ICollection<string> Cuisines { get; set; }
        public ICollection<string> Phones { get; set; }
    }
}
