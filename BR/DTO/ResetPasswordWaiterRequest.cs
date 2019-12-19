using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class ResetPasswordWaiterRequest
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Code { get; set; }
    }
}