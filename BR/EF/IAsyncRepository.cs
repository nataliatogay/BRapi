using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.EF
{
    public interface IAsyncRepository
    {
        Task<IEnumerable<Client>> GetClients();
        Task<Client> GetClientById(int id);
        Task<Client> GetClientByEmail(string email);
        Task<Client> AddClient(Client client);
        Task UpdateClient(Client client);
        Task DeleteClient(Client client);
        Task<IEnumerable<ToBeClient>> GetToBeClients();
        Task<ToBeClient> GetToBeClient(int id);
        Task AddToBeClient(ToBeClient toBeClient);
        Task UpdateToBeClient(ToBeClient toBeClient);

        Task<AdminAccountToken> GetAdminToken(string adminRefreshToken);
        Task<AdminAccountToken> GetAdminToken(int adminId);
        Task AddAdminToken(AdminAccountToken token);
        Task RemoveAdminToken(AdminAccountToken adminRefreshToken);
        Task<Admin> GetAdminById(int id);
        Task<Admin> GetAdminByEmail(string email);
        Task<Admin> AddAdmin(Admin admin);

        Task AddClientToken(ClientAccountToken token);
        Task<ClientAccountToken> GetClientToken(string clientRefreshToken);
        Task RemoveClientToken(ClientAccountToken clientRefreshToken);
    }
}
