using BR.Models;
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
        Task AddNewClient(Client client);
        Task<Client> UpdateClient(Client client);
        Task<bool> DeleteClient(int id);
        Task<IEnumerable<ToBeClient>> GetAllToBeClients();

        Task<ToBeClient> GetToBeClient(int id);
        Task AddNewToBeClient(ToBeClient toBeClient);
        Task<int> ToBeClientCount();
        string GeneratePassword();

    }
}
