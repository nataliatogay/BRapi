using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IWaiterAccountService
    {
        Task<LogInResponse> LogIn(string userName, string identityId, string notificationTag);
        Task LogOut(string identityId);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<Waiter> GetWaiter(string identityId);
    }
}
