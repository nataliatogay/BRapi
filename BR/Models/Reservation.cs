﻿using Microsoft.AspNetCore.Identity;
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

        public int? UserId { get; set; }

        [Required]
        public bool ChildFree { get; set; }

        [Required]
        public int GuestCount { get; set; }

        public string Comments { get; set; }

        public int Duration { get; set; } // in min

        // [Required]
        public int? ReservationStateId { get; set; }
        public int? CancelReasonId { get; set; }
        public string CancelledByIdentityUserId { get; set; }
        public string AdditionalInfo { get; set; }

        public virtual User User { get; set; }

        [ForeignKey("ReservationStateId")]
        public virtual ReservationState ReservationState { get; set; }
        [ForeignKey("CancelReasonId")]
        public virtual CancelReason CancelReason { get; set; }
        [ForeignKey("CancelledByIdentityUserId")]
        public virtual IdentityUser CancelledByIdentityUser { get; set; }
        public virtual ICollection<Invitee> Invitees { get; set; }
        public virtual ICollection<TableReservation> TableReservations { get; set; }

        public Reservation()
        {
            Invitees = new HashSet<Invitee>();
            TableReservations = new HashSet<TableReservation>();
        }
    }
}
