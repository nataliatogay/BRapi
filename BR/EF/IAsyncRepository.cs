﻿using BR.Models;
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
        Task<Client> GetClientByBarId(int barId);
        Task<IEnumerable<Client>> GetClients();
        Task<ClientFavourite> GetFavourite(int clientId, int userId);
        Task<IEnumerable<ClientFavourite>> GetFavourites(int userId);
        Task<ClientFavourite> AddFavourite(ClientFavourite clientFav);
        Task<bool> DeleteFavourite(ClientFavourite clientFav);
        Task<ClientImage> GetClientImage(int id);
        Task<ClientImage> AddClientImage(ClientImage image);
        Task<bool> DeleteClientImage(ClientImage image);
        Task<IEnumerable<Client>> GetClientsByMeal(string mealType);
        Task<IEnumerable<Client>> GetClientsByName(string title);
        Task<Client> UpdateClient(Client client);
        Task AddClientClientType(ClientClientType clientClientType);
        Task AddClientMealType(ClientMealType clientMealType);
        Task AddClientCuisine(ClientCuisine clientCuisine);
        Task AddClientSocialLink(SocialLink socialLink);
        Task AddClientPhone(ClientPhone clientPhone);


        // Admins

        Task<Admin> AddAdmin(Admin admin);
        Task<Admin> GetAdminByIdentityId(string identityId);
        Task<Admin> GetAdminByIdentityName(string identityName);


        // Owners

        Task<Owner> AddOwner(Owner owner);

        // Roles

        Task<IdentityRole> GetRole(int id);


        // Requests

        Task<ICollection<ClientRequest>> GetClientRequests();
        Task<ClientRequest> GetClientRequest(int id);
        Task UpdateClientRequest(ClientRequest clientRequest);
        Task AddClientRequest(ClientRequest clientRequest);


        // Tokens

        Task<IEnumerable<AccountToken>> GetTokens(string identityId);
        Task<IEnumerable<AccountToken>> GetTokens(int id);
        Task<AccountToken> GetToken(string refreshToken);
        Task<AccountToken> GetTokenByTag(string notificationTag);
        Task AddToken(AccountToken refreshToken);
        Task RemoveToken(AccountToken refreshToken);



        // Parameters

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
        Task<Waiter> GetWaiter(string identityId);
        Task<Waiter> AddWaiter(Waiter waiter);
        Task UpdateWaiter(Waiter waiter);
        Task<bool> DeleteWaiter(Waiter waiter);


        // Reservations

        Task<Reservation> AddReservation(Reservation reservation);
        Task<Reservation> UpdateReservation(Reservation reservation);
        Task<Reservation> GetReservation(int id);
        Task<ICollection<Reservation>> GetReservations(int userId);
        Task<IEnumerable<TableReservation>> GetTableReservations(int reservationId);
        Task<TableReservation> AddTableReservation(TableReservation tableReservation);
        Task DeleteTableReservations(int reservationId);
        Task<ReservationState> GetReservationState(string title);
        Task<CancelReason> GetCancelReason(int id);


        // Tables

        Task<Table> GetTable(int id);
        Task<Table> AddTable(Table table);
        Task<Table> UpdateTable(Table table);
        Task<bool> DeleteTable(Table table);
        //    Task<TableState> GetTableState(string title);


        // Bars

        Task<BarTable> GetBarTable(int id);


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