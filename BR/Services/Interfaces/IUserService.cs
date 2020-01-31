using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<UserInfoResponse>> GetUsers(string role);
        Task<UserInfoResponse> GetUser(int id, string role);
        Task<User> BlockUser(BlockUserRequest blockRequest);
    }
}
