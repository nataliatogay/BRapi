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

        public string IdentityUserId { get; set; }

        public DateTime ReservationDate { get; set; }

        public int Duration { get; set; } // in min

        public int TableId { get; set; }

        public int ClientId { get; set; }

        public bool ChildFree { get; set; }

        public bool PetsFree { get; set; }

        public bool Invalids { get; set; }

        public int GuestCount { get; set; }

        public string Comments { get; set; }

        public int? ReservationStateId { get; set; }
        
        public int? CancelReasonId { get; set; }
        
        public string CancelledByIdentityId { get; set; }

        [Required]
        public string AddedByIdentityId { get; set; }

        public int? ReservationRequestId { get; set; }

        [ForeignKey("IdentityUserId")]
        public virtual IdentityUser IdentityUser { get; set; }

        [ForeignKey("TableId")]
        public virtual Table Table { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client{ get; set; }

        [ForeignKey("ReservationStateId")]
        public virtual ReservationState ReservationState { get; set; }
        
        [ForeignKey("CancelReasonId")]
        public virtual CancelReason CancelReason { get; set; }
        
        [ForeignKey("CancelledByIdentityId")]
        public virtual IdentityUser CancelledByIdentity { get; set; }

        [ForeignKey("AddedByIdentityId")]
        public virtual IdentityUser AddedByIdentity { get; set; }

        [ForeignKey("ReservationRequestId")]
        public virtual ReservationRequest ReservationRequest { get; set; }

        public virtual ICollection<Invitee> Invitees { get; set; }
        
        public Reservation()
        {
            Invitees = new HashSet<Invitee>();
        }
    }
}
