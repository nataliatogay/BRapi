using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class NewBarReservationByPhoneRequest
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string StartDateTime { get; set; } // dd/MM/yyyy HH:mm
        public int Duration { get; set; } // in min
        [Required]
        public int GuestCount { get; set; }
        public string Comments { get; set; }
        public string PhoneNumber { get; set; }
        public int BarId { get; set; }
    }
}
