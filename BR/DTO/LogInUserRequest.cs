using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class LogInUserRequest
    {
        public string PhoneNumber { get; set; }
        public string Code { get; set; }
    }
}
