using BR.DTO;
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

        Task<ServerResponse<ClientFullInfoForClients>> GetProfileInfo(string clientIdentityId);

        Task LogOut(string refreshToken);

        Task<ServerResponse<LogInClientOwnerResponse>> UpdateToken(string refreshToken);

        Task<ServerResponse<bool>> ClientIsConfirmed(string identityId);

        Task<ServerResponse<bool>> ClientIsBlocked(string identityId);

        Task<ServerResponse<bool>> ClientIsDeleted(string identityId);

        Task<ServerResponse> UpdateClient(UpdateClientProfileRequest updateRequest, string identityIdClient);

        Task<ServerResponse<string>> UploadLogo(string identityId, string logoString);

        Task<ServerResponse> DeleteImage(int imageId);

        Task<ServerResponse<ICollection<ClientImageInfo>>> UploadImages(string identityId, ICollection<string> imagesString);

        Task<ServerResponse> SetAsMainImage(int imageId);

        Task<ServerResponse<string>> GetClientName(int clientId, string ownerIdentityId);



        // ==================================================================================================

    }
}
