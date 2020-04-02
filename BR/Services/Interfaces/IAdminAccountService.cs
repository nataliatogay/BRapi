﻿using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Admin;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IAdminAccountService
    {
        Task<ServerResponse<LogInResponse>> LogIn(string userName, string notificationTag);
        
        Task LogOut(string notificationTag);
        
        Task LogOut(IdentityUser identityUser);
        
        Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken);

        Task<ServerResponse<AdminInfoResponse>> GetAdminInfo(string identityId);
        
        Task<Admin> AddNewAdmin(Admin admin);

    }
}
