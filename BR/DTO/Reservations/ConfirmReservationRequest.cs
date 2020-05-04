﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class ConfirmReservationRequest
    {
        public int ReservationRequestId { get; set; }

        public bool Acceptance { get; set; }

        public int UserId { get; set; }

        public bool IsChildFree { get; set; }

        public bool IsPetsFree { get; set; }

        public bool Invalids { get; set; }

        public ICollection<int> InviteeIds { get; set; }

        [Required]
        public string StartDateTime { get; set; } // dd/MM/yyyy HH:mm

        [Required]
        public string EndDateTime { get; set; } // dd/MM/yyyy HH:mm

        //public int Duration { get; set; } // in min

        public string Comments { get; set; }

        public int GuestCount { get; set; }

        public int TableId { get; set; }
    }
}
