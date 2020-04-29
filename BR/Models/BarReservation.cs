using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class BarReservation
    {
        public int Id { get; set; }

        public string IdentityUserId { get; set; }

        public DateTime ReservationDate { get; set; }

        public int Duration { get; set; } // in min

        public int BarId { get; set; }

        public int ClientId { get; set; }

        public int GuestCount { get; set; }

        public string Comments { get; set; }

        public int? ReservationStateId { get; set; }

        public int? CancelReasonId { get; set; }

        public string CancelledByIdentityId { get; set; }

        [Required]
        public string AddedByIdentityId { get; set; }

        public int? BarReservationRequestId { get; set; }

        [ForeignKey("IdentityUserId")]
        public virtual IdentityUser IdentityUser { get; set; }

        [ForeignKey("BarId")]
        public virtual Bar Bar{ get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ReservationStateId")]
        public virtual ReservationState ReservationState { get; set; }

        [ForeignKey("CancelReasonId")]
        public virtual CancelReason CancelReason { get; set; }

        [ForeignKey("CancelledByIdentityId")]
        public virtual IdentityUser CancelledByIdentity { get; set; }

        [ForeignKey("AddedByIdentityId")]
        public virtual IdentityUser AddedByIdentity { get; set; }

        [ForeignKey("BarReservationRequestId")]
        public virtual BarReservationRequest BarReservationRequest { get; set; }

        public virtual ICollection<Invitee> Invitees { get; set; }

        public BarReservation()
        {
            Invitees = new HashSet<Invitee>();
        }
    }
}
