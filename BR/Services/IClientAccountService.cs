using BR.DTO;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IClientAccountService
    {
        Task<LogInResponse> LogIn(IdentityUser identityUser); 
        Task LogOut(string refreshToken);
        Task<LogInResponse> UpdateToken(string refreshToken);
        Task<Client> GetInfo(string identityId);
        Task<bool> ClientIsBlocked(string identityId);
    }
}
