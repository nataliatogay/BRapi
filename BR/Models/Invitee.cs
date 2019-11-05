using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class Invitee
    {
        [Key]
        [Column(Order = 1)]
        public int ReservationId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int UserId { get; set; }

        [ForeignKey("ReservationId")]
        public virtual Reservation Reservation { get; set; }

        [ForeignKey("UserId")]
        public virtual User User { get; set; }
    }
}
