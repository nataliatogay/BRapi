using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class BarReservationRequest
    {
        public int Id { get; set; }

        [Required]
        public string RequestedByIdentityId { get; set; }

        public DateTime ReservationDateTime { get; set; }

        public int Duration { get; set; } // in mins

        public int BarId { get; set; }

        public int GuestCount { get; set; }

        public string Comments { get; set; }

        public string ReviewedByIndentityId { get; set; }

        public int? ReservationRequestStateId { get; set; }

        [ForeignKey("BarId")]
        public virtual Bar Bar { get; set; }

        [ForeignKey("RequestedByIdentityId")]
        public virtual IdentityUser RequestedByIdentityUser { get; set; }

        [ForeignKey("ReviewedByIndentityId")]
        public virtual IdentityUser ReviewedByIdentityUser { get; set; }

        [ForeignKey("ReservationRequestStateId")]
        public virtual ReservationRequestState ReservationRequestState { get; set; }

        public virtual BarReservation BarReservation { get; set; }

    }
}


