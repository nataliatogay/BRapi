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
        Task<bool> DeleteClient(Client client);
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
        Task<IEnumerable<AccountToken>> GetTokens(string identityId);
        Task<AccountToken> GetToken(string refreshToken);
        Task AddToken(AccountToken refreshToken);
        Task RemoveToken(AccountToken refreshToken);

        Task AddClientPaymentType(int clientId, int paymentTypeId);
        Task AddClientClientType(int clientId, int clientTypeId);
        Task AddClientMealType(int clientId, int mealtTypeId);
        Task AddClientCuisine(int clientId, int cuisineId);
        Task AddClientSocialLink(int clientId, string link);
        Task AddClientPhone(int clientId, string phoneNumber, bool isShow);


        
        Task<IEnumerable<PaymentType>> GetAllPaymentTypes();
        Task<IEnumerable<MealType>> GetAllMealTypes();
        Task<IEnumerable<Cuisine>> GetAllCuisines();
        Task<IEnumerable<ClientType>> GetAllClientTypes();
        Task<Cuisine> AddCuisine(Cuisine cuisine);
        Task<Cuisine> GetCuisine(int id);
        Task<Cuisine> GetCuisine(string cuisineTitle);
        Task<Cuisine> UpdateCuisine(Cuisine cuisine);
        Task<bool> DeleteCuisine(Cuisine cuisine);
        Task<ClientType> AddClientType(ClientType clientType);
        Task<ClientType> GetClientType(int id);
        Task<ClientType> GetClientType(string clientTypeTitle);
        Task<ClientType> UpdateClientType(ClientType clientType);
        Task<bool> DeleteClientType(ClientType clientType);
        Task<PaymentType> AddPaymentType(PaymentType paymentType);
        Task<PaymentType> GetPaymentType(int id);
        Task<PaymentType> GetPaymentType(string paymentTypeTitle);
        Task<PaymentType> UpdatePaymentType(PaymentType paymentType);
        Task<bool> DeletePaymentType(PaymentType paymentType);


        Task<User> GetUser(int id);
        Task<User> GetUser(string identityId);
        Task<User> AddUser(User user);



        Task<IEnumerable<Waiter>> GetWaitersByClientId(int clientId);
        Task<Waiter> GetWaiter(int id);
        Task<Waiter> AddWaiter(Waiter waiter);
        Task UpdateWaiter(Waiter waiter);
        Task<bool> DeleteWaiter(Waiter waiter);
    }
}

/*
 Task<IEnumerable<UserMail>> GetAllUserMailsByAdminId(int adminId);
        Task<UserMail> GetUserMail(int mailId);
        Task AddUserMail(UserMail userMail);
        Task DeleteUserMail(UserMail userMail);

        Task<IEnumerable<ClientMail>> GetAllClientMailsByAdminId(int clientId);
        Task<ClientMail> GetClientMail(int mailId);
        Task AddClientMail(ClientMail clientMail);
        Task DeleteClientMail(ClientMail clientMail);
 */
