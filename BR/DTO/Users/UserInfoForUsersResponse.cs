using BR.DTO.Reservations;
using System;
using System.Collections.Generic;

namespace BR.DTO.Users
{
   

    public class UserInfoForUsersResponse 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string ImagePath { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
    }

    

    
}
