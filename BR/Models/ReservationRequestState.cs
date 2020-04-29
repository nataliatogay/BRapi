using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class ReservationRequestState
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<ReservationRequest> ReservationRequests{ get; set; }

        public virtual ICollection<BarReservationRequest> BarReservationRequests { get; set; }


        public ReservationRequestState()
        {
            ReservationRequests = new HashSet<ReservationRequest>();
            BarReservationRequests = new HashSet<BarReservationRequest>();
        }
    }
}

// accepted
// rejected
// missed

