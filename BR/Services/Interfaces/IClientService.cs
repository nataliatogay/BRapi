using BR.DTO;
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
        Task<IEnumerable<ClientInfoResponse>> GetAllClients(string role);
        Task<IEnumerable<ClientInfoResponse>> GetClientsByMeal(string mealType, string role);
        Task<IEnumerable<ClientInfoResponse>> GetClientsByName(string title, string role);
        Task<ClientInfoResponse> GetClient(int id, string role);
        Task<ClientHallsInfoResponse> GetClientHalls(int id);
        Task AddNewClient(NewClientRequest newClientRequest, string identityId);
        Task<Client> UpdateClient(Client client);
        Task<bool> DeleteClient(int id);        
        string GeneratePassword();
        
    }
}
