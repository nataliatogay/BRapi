using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Users;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IUserAccountService
    {
        Task<ServerResponse<UserInfoForUsers>> Register(NewUserRequest newUserRequest, string identityUserId);
        
        Task<ServerResponse<LogInUserResponse>> LogIn(string userName, string identityId, string notificationTag);
        
        Task LogOut(string identityId);
        
        Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken);
        
        Task<ServerResponse<bool>> UserIsBlocked(string identityId);
        
        Task<ServerResponse<bool>> UserIsDeleted(string identityId);

        Task<ServerResponse<bool>> UserIsRegistered(string identityId);
        
        Task<ServerResponse<string>> UploadImage(string identityId, string imageString);

        Task<ServerResponse> DeleteImage(string identityId);

        Task<ServerResponse<UserInfoForUsers>> UpdateProfile(UpdateUserRequest updateUserRequest, string identityId);


        // =======================================================================
        
        Task<ServerResponse> DeleteUser(string identityId);

        Task<ServerResponse> RestoreUser(string identityId);

        Task<ServerResponse> FinallyDelete(string notificationTag);

    }
}
