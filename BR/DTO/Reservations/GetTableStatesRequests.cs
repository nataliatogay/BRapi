using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class GetTableStatesRequests
    {
        public DateTime StartDateTime { get; set; }
        public int Duration { get; set; } // in min
        public ICollection<int> TableIds { get; set; }
    }
}
