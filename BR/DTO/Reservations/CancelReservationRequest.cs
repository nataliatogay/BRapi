using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class CancelReservationRequest
    {
        public int ReservationId { get; set; }
        public int CancelReasonId  { get; set; }
    }
}
