using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class BarTable
    {
        public int Id { get; set; }
        public int MaxGuestCount { get; set; }
        public int HallId { get; set; }
        public virtual Hall Hall { get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

        public BarTable()
        {
            Reservations = new HashSet<Reservation>();
        }
    }
}
