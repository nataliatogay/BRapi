using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Users
{
    public class UserShortInfoForClient
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDate { get; set; }

        public string PhoneNumber { get; set; }

        public DateTime RegistrationDate { get; set; }
    }
}
