using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class NewUserRequest
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }

        public string BirthDate { get; set; }
        public string Email { get; set; }
    }
}
