﻿using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Clients;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IClientAccountService
    {
        Task<ServerResponse<LogInResponse>> LogIn(string userName, string notificationTag);
        Task LogOut(string refreshToken);
        Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken);
        Task<Client> GetInfo(string identityId);
        Task<ServerResponse<bool>> ClientIsBlocked(string identityId);
        Task<ServerResponse<bool>> ClientIsDeleted(string identityId);
        Task<ServerResponse<string>> UploadMainImage(string identityId, string imageString);
        Task<ServerResponse> DeleteImage(int imageId);
        Task<ServerResponse> UploadImages(string identityId, ICollection<string> imagesString);

        Task<ServerResponse> UpdateClient(UpdateClientRequest updateRequest, string identityIdClient);
    }
}
