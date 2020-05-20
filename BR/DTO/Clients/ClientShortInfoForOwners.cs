using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientShortInfoForOwners
    {
        public int Id { get; set; }

        public string ClientName { get; set; }

        public string Email { get; set; }

        public string LogoPath { get; set; }

        public string MainImagePath { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? Confirmed { get; set; }

        public DateTime? Blocked { get; set; }

        public DateTime? Deleted { get; set; }
    }
}
