using BR.DTO;
using BR.DTO.Users;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IUserService
    {
        Task<ServerResponse<ICollection<UserShortInfoForAdminResponse>>> GetUserShortInfoForAdmin();
        Task<ServerResponse<UserInfoForUsersResponse>> GetUserInfoForUsers(int id);
        Task<ServerResponse<UserInfoForAdminResponse>> GetUserInfoForAdmin(int id);
        Task<ServerResponse> BlockUser(int userId);
        Task<ServerResponse> UnblockUser(int userId);
    }
}
