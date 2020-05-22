using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class Table
    {
        public int Id { get; set; }

        [Required]
        public string Code{ get; set; }

        [Required]
        public int HallId { get; set; }

        [Required]
        public int MaxGuests { get; set; }

        [Required]
        public int MinGuests { get; set; }

        public virtual Hall Hall { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }

        public virtual ICollection<ReservationRequest> ReservationRequests { get; set; }

        public Table()
        {
            Reservations = new HashSet<Reservation>();
            ReservationRequests = new HashSet<ReservationRequest>();
        }
    }
}
