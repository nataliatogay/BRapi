using BR.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.EF
{
    public interface IAsyncRepository
    {
        Task<IdentityUser> GetIdentityUser(string id);

        // Clients

        Task<Client> AddClient(Client client);
        Task DeleteClient(Client client);
        Task<Client> GetClient(int id);
        Task<Client> GetClient(string identityId);
        Task<IEnumerable<Client>> GetClients();
        Task<int> GetComingSoonCount();
        Task<IEnumerable<Client>> GetComingSoon(int skip, int take);
        Task<ClientFavourite> GetFavourite(int clientId, int userId);
        Task<ClientFavourite> AddFavourite(ClientFavourite clientFav);
        Task<bool> DeleteFavourite(ClientFavourite clientFav);
        Task<ClientGalleryImage> GetClientImage(int id);
        Task<ClientGalleryImage> AddClientImage(ClientGalleryImage image);
        Task<ClientGalleryImage> UpdateClientImage(ClientGalleryImage image);
        Task AddClientImages(ICollection<ClientGalleryImage> images);
        Task<bool> DeleteClientImage(ClientGalleryImage image);
        Task<IEnumerable<Client>> GetClientsByMeal(string mealType);
        Task<IEnumerable<Client>> GetClientsByName(string title);
        Task<Client> UpdateClient(Client client);

        Task AddClientClientType(ClientClientType clientClientType);
        Task AddClientMealType(ClientMealType clientMealType);
        Task AddClientCuisine(ClientCuisine clientCuisine);
        Task AddClientDish(ClientDish clientDish);
        Task AddClientGoodFor(ClientGoodFor clientGoodFor);
        Task AddClientFeature(ClientFeature clientFeature);
        Task AddClientSpecialDiet(ClientSpecialDiet clientSpecialDiet);
        Task AddClientSocialLink(SocialLink socialLink);
        Task AddClientPhone(ClientPhone clientPhone);

        Task RemoveClientClientType(IEnumerable<ClientClientType> clientClientType);
        Task RemoveClientMealType(IEnumerable<ClientMealType> clientMealType);
        Task RemoveClientCuisine(IEnumerable<ClientCuisine> clientCuisine);
        Task RemoveClientDish(IEnumerable<ClientDish> clientDish);
        Task RemoveClientGoodFor(IEnumerable<ClientGoodFor> clientGoodFor);
        Task RemoveClientFeature(IEnumerable<ClientFeature> clientFeature);
        Task RemoveClientSpecialDiet(IEnumerable<ClientSpecialDiet> clientSpecialDiet);
        Task RemoveClientSocialLink(IEnumerable<SocialLink> socialLink);
        Task RemoveClientPhone(IEnumerable<ClientPhone> clientPhone);


        // Admins

        Task<Admin> AddAdmin(Admin admin);
        Task<Admin> GetAdmin(string identityId);
        Task<Admin> GetAdminByIdentityName(string identityName);
        Task<ICollection<Admin>> GetAdmins();


        // Owners

        Task<Owner> AddOwner(Owner owner);
        Task<Owner> GetOwner(string identityId);
        Task<Owner> GetOwner(int id);
        Task<Owner> UpdateOwner(Owner owner);


        // Organizations

        Task<ICollection<Organization>> GetOrganizations();
        Task<Organization> GetOrganization(int id);
        Task<Organization> AddOrganization(Organization organization);
        Task<Organization> UpdateOrganization(Organization organization);


        // Roles

        Task<IdentityRole> GetRole(int id);


        // Requests

        Task<ICollection<OwnerRequest>> GetClientRequests();
        Task<ICollection<OwnerRequest>> GetClientRequests(int take, int skip);
        //  Task<ICollection<ClientRequest>> GetUndoneClientRequests();
        Task<OwnerRequest> GetClientRequest(int id);
        Task UpdateClientRequest(OwnerRequest clientRequest);
        Task<OwnerRequest> AddClientRequest(OwnerRequest clientRequest);


        // Tokens

        Task<IEnumerable<AccountToken>> GetTokens(string identityId);
        Task<AccountToken> GetToken(string refreshToken);
        Task<AccountToken> GetTokenByTag(string notificationTag);
        Task AddToken(AccountToken refreshToken);
        Task RemoveToken(AccountToken refreshToken);


        // Privileges

        Task<IEnumerable<UserPrivileges>> GetUserPrivileges(string identityId);
        Task<UserPrivileges> AddUserPrivilage(UserPrivileges userPrivileges);
        Task<Privilege> GetPrivilege(int id);



        // Parameters

        Task<ICollection<MealType>> GetAllMealTypes();

        Task<ICollection<Cuisine>> GetAllCuisines();
        Task<Cuisine> AddCuisine(Cuisine cuisine);
        Task<Cuisine> GetCuisine(string cuisineTitle);
        Task<Cuisine> GetCuisine(int id);
        Task<Cuisine> UpdateCuisine(Cuisine cuisine);
        Task DeleteCuisine(Cuisine cuisine);

        Task<ICollection<ClientType>> GetAllClientTypes();
        Task<ClientType> GetClientType(int id);
        Task<ClientType> GetClientType(string clientTypeTitle);
        Task<ClientType> AddClientType(ClientType clientType);
        Task<ClientType> UpdateClientType(ClientType clientType);
        Task DeleteClientType(ClientType clientType);

        Task<ICollection<GoodFor>> GetAllGoodFors();
        Task<GoodFor> GetGoodFor(int id);
        Task<GoodFor> GetGoodFor(string title);
        Task<GoodFor> AddGoodFor(GoodFor goodFor);
        Task<GoodFor> UpdateGoodFor(GoodFor goodFor);
        Task DeleteGoodFor(GoodFor goodFor);

        Task<ICollection<Feature>> GetAllFeatures();
        Task<Feature> GetFeature(int id);
        Task<Feature> GetFeature(string title);
        Task<Feature> AddFeature(Feature feature);
        Task<Feature> UpdateFeature(Feature feature);
        Task DeleteFeature(Feature feature);

        Task<ICollection<SpecialDiet>> GetAllSpecialDiets();
        Task<SpecialDiet> GetSpecialDiet(int id);
        Task<SpecialDiet> GetSpecialDiet(string title);
        Task<SpecialDiet> AddSpecialDiet(SpecialDiet specialDiet);
        Task<SpecialDiet> UpdateSpecialDiet(SpecialDiet specialDiet);
        Task DeleteSpecialDiet(SpecialDiet specialDiet);

        Task<ICollection<Dish>> GetAllDishes();
        Task<Dish> GetDish(int id);
        Task<Dish> GetDish(string dish);
        Task<Dish> AddDish(Dish dish);
        Task<Dish> UpdateDish(Dish dish);
        Task DeleteDish(Dish dish);




        // Users

        Task<User> GetUser(int id);
        Task<User> GetUser(string identityId);
        Task<User> AddUser(User user);
        Task<User> UpdateUser(User user);
        Task<ICollection<User>> GetUsers();
        Task<bool> DeleteUser(User user);


        // Waiters

        Task<IEnumerable<Waiter>> GetWaiters();
        Task<Waiter> GetWaiter(int id);
        Task<Waiter> GetWaiter(string identityId);
        Task<Waiter> AddWaiter(Waiter waiter);
        Task<Waiter> UpdateWaiter(Waiter waiter);
        Task<bool> DeleteWaiter(Waiter waiter);


        // Reservations

        Task<Reservation> GetReservation(int id);
        Task<Reservation> AddReservation(Reservation reservation);
        Task<Reservation> UpdateReservation(Reservation reservation);
        Task<ReservationState> GetReservationState(string title);
        Task<CancelReason> GetCancelReason(int id);
        Task<CancelReason> GetCancelReason(string title);
        Task<ICollection<Reservation>> GetAllUserReservations(string identityId);


        // Reservation Requests

        Task<ReservationRequest> GetReservationRequest(int id);
        Task<ReservationRequest> AddReservationRequest(ReservationRequest reservationRequest);
        Task<ReservationRequest> UpdateReservationRequest(ReservationRequest reservationRequest);

        Task<ReservationRequestState> GetReservationRequestState(string title);

        Task<ICollection<ReservationRequest>> GetWaitingReservationRequestsByClientId(int clientId);
        Task<ICollection<ReservationRequest>> GetRejectedReservationRequestsByClientId(int clientId, DateTime date);



        // Invitees

        Task<Invitee> AddInvitee(Invitee invitee);





        // Visitors

        Task<Visitor> AddVisitor(Visitor visitor);
        Task<Visitor> GetVisitor(int id);
        Task<Visitor> UpdateVisitor(Visitor visitor);



        // Tables

        Task<Table> GetTable(int id);
        Task<Table> AddTable(Table table);
        Task<Table> UpdateTable(Table table);
        Task<bool> DeleteTable(Table table);
        Task<ICollection<Table>> GetClientTables(int clientId);

        //Task<ICollection<Table>> GetTablesByClientId(int clientId);



        // Bars

        Task<Bar> GetBar(int id);


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
        Task<ICollection<Event>> GetUpcomingEventsByClient(int clientId, int skip, int take);
        Task<int> GetUpcomingEventsByClientCount(int clientId);
        Task<Event> GetEvent(int id);
        Task<int> GetUpcomingEventsByNameCount(string title);
        Task<IEnumerable<Event>> GetUpcomingEventsByName(string title, int skip, int take);
        Task<IEnumerable<Event>> GetEventsByNameAndDescription(string text);
        Task<Event> AddEvent(Event clientEvent);
        Task<Event> UpdateEvent(Event clientEvent);
        Task RemoveEvent(Event clientEvent);

        Task<EventMark> GetEventMark(int eventId, int userId);
        Task<EventMark> AddEventMark(EventMark eventMark);
        Task RemoveEventMark(EventMark eventMark);


        // Notifications

        Task<NotificationType> GetNotificationType(string title);
        Task<NotificationType> GetNotificationType(int id);
        Task<AdminNotification> AddAdminNotification(AdminNotification adminNotification);
        Task<ICollection<AdminNotification>> GetAdminNotifications();
        Task<ICollection<AdminNotification>> GetUndoneAdminNotifications();
        Task<ICollection<AdminNotification>> GetAdminNotifications(int take, int skip);
        Task<AdminNotification> GetAdminNotification(int id);
        Task<AdminNotification> UpdateAdminNotification(AdminNotification adminNotification);




    }
}