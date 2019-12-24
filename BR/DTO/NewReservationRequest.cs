using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class NewReservationRequest
    {
        public string ReservationDate { get; set; } // format - ?? dd/MM/yyyy hh:mm
        public bool IsChildFree { get; set; }
        public int GuestCount { get; set; }
        public string Comments { get; set; }
        public ICollection<int> TableIds { get; set; }
    }
}
