using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class TableState
    {
        [Required]
        public string StartDateTime { get; set; }   // dd/MM/yyyy HH:mm

        [Required]
        public int Duration { get; set; } // in min

        [Required]
        public int TableId { get; set; }
    }
}
