using BR.DTO;
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
        Task<LogInResponse> LogIn(string userName, string identityId);
        Task LogOut(string identityId);
        Task LogOut(IdentityUser identityUser);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<Admin> GetAdmin(string identityId);
        Task<Admin> AddNewAdmin(Admin admin);

    }
}
