using BR.DTO;
using BR.DTO.Clients;
using BR.DTO.Schema;
using BR.DTO.Users;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IClientService
    {
        Task AddNewClient(NewClientRequest newClientRequest, string identityId);
        Task<bool> DeleteClient(int id);
        Task<ICollection<ClientShortInfoForUsersResponse>> GetShortClientInfoForUsers();
        Task<ICollection<ClientShortInfoForAdminResponse>> GetShortClientInfoForAdmin();
        Task<ClientFullInfoForAdminResponse> GetFullClientInfoForAdmin(int id);
        Task<ClientFullInfoForUsersResponse> GetFullClientInfoForUsers(int id);
        Task<ICollection<ClientFullInfoForUsersResponse>> GetFullClientInfoForUsers();
        Task<ICollection<ClientShortInfoForUsersResponse>> GetFavourites(string identityUserId);
        Task<bool> AddFavourite(int clientId, string identityUserId);
        Task<bool> DeleteFavourite(int clientId, string identityUserId);
        Task<ICollection<ClientFullInfoForUsersResponse>> GetClientsByMeal(string mealType);
        Task<IEnumerable<ClientFullInfoForUsersResponse>> GetClientsByName(string title);
        Task<ClientFullInfoForAdminResponse> GetClientForAdmin(int id);
        Task<ClientHallsInfoResponse> GetClientHalls(int id);
        Task<Client> UpdateClient(Client client);
        Task<Client> BlockClient(BlockUserRequest blockRequest);
        string GeneratePassword();
    }
}
