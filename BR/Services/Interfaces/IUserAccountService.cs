using BR.DTO;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<UserInfoResponse> Register(User user);       
        Task<LogInUserResponse> LogIn(string userName, string identityId, string notificationTag);
        Task LogOut(string identityId);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<UserInfoResponse> GetInfo(string identityId);
        Task<bool> UserIsBlocked(string identityId);
        Task<string> UploadImage(string identityId, string imageString);
        Task<UserInfoResponse> UpdateProfile(UpdateUserRequest updateUserRequest, string identityId);
        Task<bool> DeleteUser(string identityId);
        string GenerateCode();
    }
}
