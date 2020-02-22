using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class Table
    {
        public int Id { get; set; }

        [Required]
        public int Number { get; set; }

        [Required]
        public int HallId { get; set; }

        [Required]
        public int MaxGuests { get; set; }

        [Required]
        public int MinGuests { get; set; }

     //   [Required]
      //  public int TableStateId { get; set; }

        public virtual Hall Hall { get; set; }

      //  public virtual TableState TableState { get; set; }

        public virtual ICollection<TableReservation> TableReservations { get; set; }

        public Table()
        {
            TableReservations = new HashSet<TableReservation>();
        }
    }
}
