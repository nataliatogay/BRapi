using BR.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
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
        Task<Admin> GetAdminByIdentityName(string identityName);
        Task<Client> GetClientByIdentityId(string identityId);
        Task<IEnumerable<ClientRequest>> GetClientRequests();
        Task<ClientRequest> GetClientRequest(int id);
        Task UpdateClientRequest(ClientRequest clientRequest);
        Task AddClientRequest(ClientRequest clientRequest);
        Task<Admin> AddAdmin(Admin admin);
        Task<AccountToken> GetToken(string refreshToken);
        Task AddToken(AccountToken refreshToken);
        Task RemoveToken(AccountToken refreshToken);

        Task AddClientPaymentType(int clientId, int paymentTypeId);
        Task AddClientClientType(int clientId, int clientTypeId);
        Task AddClientCuisine(int clientId, int cuisineId);
        Task AddClientSocialLink(int clientId, string link);
        Task AddClientPhone(int clientId, int phoneCodeId, string phone);


        Task<IEnumerable<UserMail>> GetAllUserMailsByAdminId(int adminId);
        Task<UserMail> GetUserMail(int mailId);
        Task AddUserMail(UserMail userMail);
        Task DeleteUserMail(UserMail userMail);

        Task<IEnumerable<ClientMail>> GetAllClientMailsByAdminId(int clientId);
        Task<ClientMail> GetClientMail(int mailId);
        Task AddClientMail(ClientMail clientMail);
        Task DeleteClientMail(ClientMail clientMail);
        Task<IEnumerable<PaymentType>> GetAllPaymentTypes();
        Task<IEnumerable<Cuisine>> GetAllCuisines();
        Task<IEnumerable<ClientType>> GetAllClientTypes();



        Task<User> GetUser(int id);
        Task<User> AddUser(User user);
    }
}
