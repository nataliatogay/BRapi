using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class TableReservation
    {
        [Key]
        [Column(Order = 1)]
        public int TableId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ReservationId { get; set; }

        [ForeignKey("TableId")]
        public virtual Table Table { get; set; }

        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }
    }
}
