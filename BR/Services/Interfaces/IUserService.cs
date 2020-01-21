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
        Task<IEnumerable<UserInfoResponse>> GetUsers();
        Task<UserInfoResponse> GetUser(int id);
        Task<User> BlockUser(int id);
    }
}
