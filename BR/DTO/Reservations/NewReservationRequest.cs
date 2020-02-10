using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class NewReservationRequest
    {
        [Required]
        public string ReservationDate { get; set; } // dd/MM/yyyy HH:mm
        [Required]
        public bool IsChildFree { get; set; }
        public int GuestCount { get; set; }
        public string Comments { get; set; }
        public ICollection<int> TableIds { get; set; }
    }
}
