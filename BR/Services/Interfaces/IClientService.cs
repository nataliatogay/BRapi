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
        Task<ServerResponse<ClientShortInfoForAdmin>> AddNewClientByAdmin(NewClientByAdminRequest newRequest, string clientIdentityId);

        Task<ServerResponse<ClientShortInfoForOwners>> AddNewClientByOwner(NewClientByOwnerRequest newRequest, string clientIdentityId, string ownerIdentityId);

        Task<ServerResponse> ConfirmClientRegistration(int clientId);

        Task<ServerResponse<string>> GetClientName(int clientId);


        Task<ServerResponse<ICollection<ClientShortInfoForAdmin>>> GetShortClientInfoForAdmin();

        Task<ServerResponse<ICollection<ClientShortInfoForOwners>>> GetShortClientInfoForOwners(string ownerIdentityId);

        Task<ServerResponse<ClientFullInfoForAdmin>> GetFullClientInfoForAdmin(int id);

        Task<ServerResponse<ClientFullInfoForOwners>> GetFullClientInfoForOwners(int clientId, string ownerIdentityId);

        Task<ServerResponse<ClientShortInfoForAdmin>> UpdateClientByAdmin(UpdateClientRequest updateRequest);

        Task<ServerResponse<ClientShortInfoForOwners>> UpdateClientByOwner(UpdateClientRequest updateRequest, string ownerIdentityId);

        Task<ServerResponse<string>> UploadLogoByAdmin(UploadLogoRequest uploadRequest);

        Task<ServerResponse<string>> UploadLogoByOwner(UploadLogoRequest uploadRequest, string ownerIdentityId);

        Task<ServerResponse> SetAsMainImageByAdmin(int imageId);

        Task<ServerResponse> SetAsMainImageByOwner(int imageId, string ownerIdentityId);

        Task<ServerResponse> BlockClient(int clientId);

        Task<ServerResponse> UnblockClient(int clientId);

        Task<ServerResponse<ICollection<ClientImageInfo>>> UploadImagesByAdmin(UploadImagesRequest uploadRequest);

        Task<ServerResponse<ICollection<ClientImageInfo>>> UploadImagesByOwner(UploadImagesRequest uploadRequest, string ownerIdentityId);

        Task<ServerResponse> DeleteImageByAdmin(int imageId);

        Task<ServerResponse> DeleteImageByOwner(int imageId, string ownerIdentityId);


        // FOR USERS
        Task<ServerResponse<ClientFullInfoForUsers>> GetFullClientInfoForUsers(int id);

        Task<ServerResponse<ICollection<int>>> GetFavouritesIds(string userIdentityId);

        Task<ServerResponse> AddFavourite(int clientId, string identityUserId);

        Task<ServerResponse> DeleteFavourite(int clientId, string identityUserId);

        Task<ServerResponse<ICollection<ClientShortInfoForUsers>>> GetComingSoon(int skip, int take);


        // ================================================================================


            // change
        Task<ServerResponse<ICollection<ClientShortInfoForUsers>>> GetClientsByFilterForUsers(ClientFilter filter, int skip, int take);







        Task<ICollection<ClientShortInfoForUsers>> GetFavourites(string identityUserId);



        Task<ICollection<ClientFullInfoForUsers>> GetClientsByMeal(string mealType);

        Task<IEnumerable<ClientFullInfoForUsers>> GetClientsByName(string title);

        Task<ICollection<ClientShortInfoForUsers>> GetShortClientInfoForUsers();



        Task<ICollection<ClientFullInfoForUsers>> GetFullClientInfoForUsers();

        Task<ClientHallsInfo> GetClientHalls(int id);





        Task<ServerResponse> DeleteClient(int clientId);
    }
}
