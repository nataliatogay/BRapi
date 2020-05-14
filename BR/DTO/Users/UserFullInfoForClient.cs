﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Users
{
    public class UserFullInfoForClient
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImagePath { get; set; }

        public bool Gender { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public string Email { get; set; }

        public DateTime RegistrationDate { get; set; }

        public int ReservationCount { get; set; }

        public DateTime? LastVisitDate { get; set; }

    }
}
