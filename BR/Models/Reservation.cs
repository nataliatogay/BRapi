using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class Reservation
    {
        public int Id { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public bool ChildFree { get; set; }

        [Required]
        public int GuestCount { get; set; }

        public string Comments { get; set; }

        [Required]
        public int ReservationStateId { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("ReservationStateId")]
        public virtual ReservationState ReservationState { get; set; }
        public virtual ICollection<Invitee> Invitees { get; set; }
        public virtual ICollection<TableReservation> TableReservations { get; set; }

        public Reservation()
        {
            Invitees = new HashSet<Invitee>();
            TableReservations = new HashSet<TableReservation>();
        }
    }
}
