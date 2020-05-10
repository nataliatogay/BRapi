using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Requests
{
    public class OwnerRequestInfo
    {
        public int Id { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OrganizationName { get; set; }

        public string Email { get; set; }

        public string Comments { get; set; }

        public DateTime RegisteredDate { get; set; }

        public int? OwnerId { get; set; }
    }
}
