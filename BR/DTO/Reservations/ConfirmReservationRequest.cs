using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class ConfirmReservationRequest
    {

        public int UserId { get; set; }
        [Required]
        public string StartDateTime { get; set; } // dd/MM/yyyy HH:mm
        public int Duration { get; set; } // in min
        public ICollection<int> TableIds { get; set; }
    }
}
