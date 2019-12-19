using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoResponse>> GetUsers();
        Task<string> UploadImage(string identityId, string imageString);
        Task<UserInfoResponse> UpdateUser(UpdateUserRequest updateUserRequest, string identityId);
    }
}
