using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Owners
{
    public class NewOwnerRequest
    {
        public string OwnerName { get; set; }

        public string Email { get; set; }

        public string OwnerNumber { get; set; }

        public int OrganizationId { get; set; }

        public int? RequestId { get; set; }
    }
}
