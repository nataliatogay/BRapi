using BR.DTO;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IClientService
    {
        Task<IEnumerable<Client>> GetAllClients();
        Task<Client> GetClient(int id);
        Task AddNewClient(NewClientRequest newClientRequest, string identityId);
        Task<Client> UpdateClient(Client client);
        Task<bool> DeleteClient(int id);
        
        string GeneratePassword();

    }
}
