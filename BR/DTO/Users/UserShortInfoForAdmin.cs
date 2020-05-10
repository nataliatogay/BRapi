﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Users
{
    public class UserShortInfoForAdmin
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string ImagePath { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime? Blocked { get; set; }

        public DateTime? Deleted { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
