using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.EF
{
    public interface IAsyncRepository
    {
        Task<IdentityUser> GetIdentityUser(string id);
        Task<Client> AddClient(Client client);
        Task DeleteClient(Client client);
        Task<Client> GetClientById(int id);
        Task<IEnumerable<Client>> GetClients();
        Task UpdateClient(Client client);
        Task<Admin> GetAdminByIdentityId(string identityId);
        Task<Client> GeClientByIdentityId(string identityId);
        Task<IEnumerable<ToBeClient>> GetToBeClients();
        Task<ToBeClient> GetToBeClient(int id);
        Task UpdateToBeClient(ToBeClient toBeClient);
        Task AddToBeClient(ToBeClient toBeClient);
        Task<Admin> AddAdmin(Admin admin);
        Task<AccountToken> GetToken(string refreshToken);
        Task AddToken(AccountToken refreshToken);
        Task RemoveToken(AccountToken refreshToken);

    }
}
