using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Account
{
    public class LogInUserRequest
    {
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public string Code { get; set; }
        [Required]
        public string NotificationTag { get; set; }
    }
}
