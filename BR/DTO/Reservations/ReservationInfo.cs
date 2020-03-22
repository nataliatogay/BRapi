using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class ReservationInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int GuestCount { get; set; }
        public bool ChildFree { get; set; }
        public string ReservationState { get; set; }
        public string ClientTitle { get; set; }
        public string HallTitle { get; set; }
        public int Floor { get; set; }
        public ICollection<int> TableNumbers { get; set; }
        public string Comments { get; set; }
    }
}
