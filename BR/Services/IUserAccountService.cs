using BR.DTO;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IUserAccountService
    {
        Task<User> Register(User user);
       
        Task<LogInResponse> LogIn(string userName, string identityId);
        Task LogOut(string refreshToken);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<User> GetInfo(string identityId);
        string GenerateCode();
    }
}
