using BR.DTO.Clients;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Requests
{
    public class NewClientRequestRequest
    {
        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Comments { get; set; }
    }
}
