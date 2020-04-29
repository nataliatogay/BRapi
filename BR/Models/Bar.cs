using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Bar
    {
        public int Id { get; set; }
        
        public int MaxGuestCount { get; set; }
        
        public int HallId { get; set; }
        
        public virtual Hall Hall { get; set; }
        
        public virtual ICollection<BarReservation> BarReservations { get; set; }

        public virtual ICollection<BarReservationRequest> BarReservationRequests { get; set; }

        public Bar()
        {
            BarReservations = new HashSet<BarReservation>();
            BarReservationRequests = new HashSet<BarReservationRequest>();
        }
    }
}
