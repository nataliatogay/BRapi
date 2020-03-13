using BR.DTO;
using BR.DTO.Account;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IAdminAccountService
    {
        Task<ServerResponse<LogInResponse>> LogIn(string userName, string identityId, string notificationTag);
        Task LogOut(string notificationTag);
        Task LogOut(IdentityUser identityUser);
        Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken);
        Task<Admin> GetAdmin(string identityId);
        Task<Admin> AddNewAdmin(Admin admin);

    }
}
