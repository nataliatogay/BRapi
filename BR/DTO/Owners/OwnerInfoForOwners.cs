using BR.DTO.Organization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Owners
{
    public class OwnerInfoForOwners
    {

        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public OrganizationInfo Organization { get; set; }
    }
}
