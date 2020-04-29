using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class TableState
    {
        public string StartDateTime { get; set; }   // dd/MM/yyyy HH:mm
        public int Duration { get; set; } // in min
        public int TableId { get; set; }
    }
}
