using BR.DTO;
using BR.DTO.Account;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IWaiterAccountService
    {
        Task<ServerResponse<LogInResponse>> LogIn(string userName, string notificationTag);
        Task LogOut(string identityId);
        Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken);
        Task<Waiter> GetWaiter(string identityId);
    }
}
