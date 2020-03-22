using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class NewBarReservationRequest
    {
        [Required]
        public string Code { get; set; }
        public string Comments { get; set; }
    }
}
