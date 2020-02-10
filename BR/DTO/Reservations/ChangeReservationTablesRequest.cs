using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class ChangeReservationTablesRequest
    {
        public int ReservationId { get; set; }
        public ICollection<int> TableIds { get; set; }
    }
}
