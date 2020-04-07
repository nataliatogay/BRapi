using BR.DTO;
using BR.DTO.Clients;
using BR.DTO.Schema;
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
    public interface IClientService
    {
        Task<ServerResponse> AddNewClient(NewClientRequest newRequest, string clientIdentityId, string addedByIdentityId, string role);
        
        Task<ICollection<ClientShortInfoForUsersResponse>> GetShortClientInfoForUsers();
        
        Task<ICollection<ClientShortInfoForAdminResponse>> GetShortClientInfoForAdmin();
        
        Task<ClientFullInfoForAdminResponse> GetFullClientInfoForAdmin(int id);
        
        Task<ClientFullInfoForUsersResponse> GetFullClientInfoForUsers(int id);
        
        Task<ICollection<ClientFullInfoForUsersResponse>> GetFullClientInfoForUsers();
        
        Task<ICollection<ClientShortInfoForUsersResponse>> GetFavourites(string identityUserId);
        
        Task<ServerResponse> AddFavourite(int clientId, string identityUserId);
        
        Task<ServerResponse> DeleteFavourite(int clientId, string identityUserId);
        
        Task<ICollection<ClientFullInfoForUsersResponse>> GetClientsByMeal(string mealType);
        
        Task<IEnumerable<ClientFullInfoForUsersResponse>> GetClientsByName(string title);
        
        Task<ClientFullInfoForAdminResponse> GetClientForAdmin(int id);
        
        Task<ClientHallsInfoResponse> GetClientHalls(int id);

        Task<ServerResponse> UpdateClient(UpdateClientRequest updateRequest);

        Task<ServerResponse<string>> UploadMainImage(UploadMainImageRequest uploadRequest);

        Task<ServerResponse> SetAsMainImage(int imageId);


        Task<ServerResponse> UploadImages(UploadImagesRequest uploadRequest);

        Task<ServerResponse> DeleteImage(int imageId);

        Task<ServerResponse> BlockClient(int clientId);
        
        Task<ServerResponse> UnblockClient(int clientId);

        Task<ServerResponse> ConfirmClient(int clientId);

        Task<ServerResponse> DeleteClient(int clientId);
    }
}
