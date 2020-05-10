using BR.DTO.Users;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Account
{
    public class LogInUserResponse
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public UserInfoForUsers User { get; set; }
    }
}
