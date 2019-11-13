using BR.DTO;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IAdminAccountService
    {
        Task<LogInResponse> LogIn(string userName, string identityId);
        Task LogOut(string refreshToken);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<Admin> GetInfo(string identityId);
        Task<Admin> AddNewAdmin(Admin admin);

    }
}
