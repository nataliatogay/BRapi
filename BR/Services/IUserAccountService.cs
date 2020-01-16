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
        Task<UserInfoResponse> Register(User user);       
        Task<LogInUserResponse> LogIn(string userName, string identityId, string notificationTag);
        Task LogOut(string identityId);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<UserInfoResponse> GetInfo(string identityId);
        Task<bool> UserIsBlocked(string identityId);
        string GenerateCode();
    }
}
