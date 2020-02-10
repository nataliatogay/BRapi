using BR.DTO;
using BR.DTO.Account;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IAdminAccountService
    {
        Task<LogInResponse> LogIn(string userName, string identityId, string notificationTag);
        Task LogOut(string notificationTag);
        Task LogOut(IdentityUser identityUser);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<Admin> GetAdmin(string identityId);
        Task<Admin> AddNewAdmin(Admin admin);

    }
}
