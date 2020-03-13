using BR.DTO.Account;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IAuthenticationService
    {
        Task<ServerResponse<LogInResponse>> Authentication(string userName, string notificationTag);
        Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken);
    }
}
