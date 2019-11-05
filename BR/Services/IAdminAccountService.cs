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
        Task<LogInResponse> LogIn(string email, string password);
        Task LogOut(string refreshToken);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<Admin> GetInfo(int id);
        Task<Admin> AddNewAdmin(IdentityUser user);

    }
}
