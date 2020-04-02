using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Requests
{
    public class RequestInfoResponse
    {
        public int Id { get; set; }

        public string OwnerName { get; set; }

        public string OwnerPhoneNumber { get; set; }

        public string OrganizationName { get; set; }

        public string Email { get; set; }

        public string Comments { get; set; }

        public bool IsDone { get; set; }

        public DateTime RegisteredDate { get; set; }
    }
}
