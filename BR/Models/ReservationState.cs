
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class ReservationState
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public ReservationState()
        {
            Reservations = new HashSet<Reservation>();
        }
    
    }
}
