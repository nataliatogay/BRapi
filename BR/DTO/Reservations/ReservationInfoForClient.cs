﻿using BR.DTO.Schema;
using BR.DTO.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Reservations
{
    public class ReservationInfoForClient
    {
        public int Id { get; set; }

        public UserFullInfoForClient User { get; set; }

        public DateTime ApplicationDate { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int GuestCount { get; set; }

        public TableInfo Table { get; set; }

        public string State { get; set; }

        public bool ChildFree { get; set; }

        public bool PetsFree { get; set; }

        public bool Invalids { get; set; }

        public string Comments { get; set; }

        public ICollection<UserFullInfoForClient> Invitees { get; set; }
    }
}
