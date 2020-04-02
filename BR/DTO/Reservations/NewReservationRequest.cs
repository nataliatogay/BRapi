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
        public string Code { get; set; }
        [Required]
        public bool IsChildFree { get; set; }
        public bool IsPetsFree { get; set; }
        public bool Invalids { get; set; }
        public int GuestCount { get; set; }
        public string Comments { get; set; }
    }
}
