using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Account
{
    public class LogInClientOwnerResponse
    {
        public string AccessToken { get; set; }

        public string RefreshToken { get; set; }

        public string Role { get; set; }
    }
}
