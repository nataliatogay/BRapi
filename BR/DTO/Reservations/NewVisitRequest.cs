using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class NewVisitRequest
    {
        public int? TableId { get; set; }

        public int? BarTableId { get; set; }

        [Required]
        public string StartDateTime { get; set; }

        public int Duration { get; set; }

        public int GuestCount { get; set; }
    }
}
