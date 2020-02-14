using BR.Models;
using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.EF
{
    public interface IAsyncRepository
    {
        Task<IdentityUser> GetIdentityUser(string id);

        // Clients

        Task<Client> AddClient(Client client);
        Task<bool> DeleteClient(Client client);
        Task<Client> GetClient(int id);
        Task<Client> GetClient(string identityId);
        Task<Client> GetClientByTableId(int tableId);
        Task<IEnumerable<Client>> GetClients();
        Task<IEnumerable<ClientFavourite>> GetFavourites(int userId);
        Task<ClientImage> GetClientImage(int id);
        Task<ClientImage> AddClientImage(ClientImage image);
        Task<bool> DeleteClientImage(ClientImage image);
        Task<IEnumerable<Client>> GetClientsByMeal(string mealType);
        Task<IEnumerable<Client>> GetClientsByName(string title);
        Task<Client> UpdateClient(Client client);
        Task AddClientPaymentType(int clientId, int paymentTypeId);
        Task AddClientClientType(int clientId, int clientTypeId);
        Task AddClientMealType(int clientId, int mealtTypeId);
        Task AddClientCuisine(int clientId, int cuisineId);
        Task AddClientSocialLink(int clientId, string link);
        Task AddClientPhone(int clientId, string phoneNumber, bool isShow);


        // Admins

        Task<Admin> AddAdmin(Admin admin);
        Task<Admin> GetAdminByIdentityId(string identityId);
        Task<Admin> GetAdminByIdentityName(string identityName);


        // Requests

        Task<ICollection<ClientRequest>> GetClientRequests();
        Task<ClientRequest> GetClientRequest(int id);
        Task UpdateClientRequest(ClientRequest clientRequest);
        Task AddClientRequest(ClientRequest clientRequest);


        // Tokens

        Task<IEnumerable<AccountToken>> GetTokens(string identityId);
        Task<AccountToken> GetToken(string refreshToken);
        Task<AccountToken> GetTokenByTag(string notificationTag);
        Task AddToken(AccountToken refreshToken);
        Task RemoveToken(AccountToken refreshToken);



        // Parameters

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



        // Users

        Task<User> GetUser(int id);
        Task<User> GetUser(string identityId);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task<IEnumerable<User>> GetUsers();
        Task<bool> DeleteUser(User user);


        // Waiters

        Task<IEnumerable<Waiter>> GetWaitersByClientId(int clientId);
        Task<IEnumerable<Waiter>> GetWaiters();
        Task<Waiter> GetWaiter(int id);
        Task<Waiter> AddWaiter(Waiter waiter);
        Task UpdateWaiter(Waiter waiter);
        Task<bool> DeleteWaiter(Waiter waiter);
        Task<Waiter> GetWaiter(string identityId);


        // Reservations

        Task<Reservation> AddReservation(Reservation reservation);
        Task<Reservation> UpdateReservation(Reservation reservation);
        Task<Reservation> GetReservation(int id);
        Task<ICollection<Reservation>> GetReservations(int userId);
        Task<IEnumerable<TableReservation>> GetTableReservations(int reservationId);
        Task<TableReservation> AddTableReservation(int reservationId, int tableId);
        Task DeleteTableReservations(int reservationId);
        Task<ReservationState> GetReservationState(string title);


        // Tables

        Task<Table> GetTable(int id);
        Task<Table> AddTable(Table table);
        Task<Table> UpdateTable(Table table);
        Task<bool> DeleteTable(Table table);
        Task<TableState> GetTableState(string title);


        // Floors
        Task<Floor> AddFloor(Floor floor);
        Task<Floor> GetFloor(int clientId, int floorNumber);


        // Halls

        Task<Hall> AddHall(Hall hall);
        Task<Hall> GetHall(int id);
        Task<Hall> UpdateHall(Hall hall);




        // Events
        Task<ICollection<Event>> GetEvents();
        Task<ICollection<Event>> GetUpcomingEvents();
        Task<ICollection<Event>> GetEventsByClient(int clientId);
        Task<ICollection<Event>> GetUpcomingEventsByClient(int clientId);
        Task<Event> GetEvent(int id);
        Task<Event> AddEvent(Event clientEvent);
        Task<Event> UpdateEvent(Event clientEvent);

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
