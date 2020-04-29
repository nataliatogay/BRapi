using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BR.EF
{
    public class EFAsyncRepository : IAsyncRepository
    {
        private readonly BRDbContext _db;

        public EFAsyncRepository(BRDbContext db)
        {
            _db = db;
        }

        public async Task<IdentityUser> GetIdentityUser(string id)
        {
            return await _db.Users.FindAsync(id);
        }


        // Clients

        public async Task<Client> AddClient(Client client)
        {
            var res = await _db.Clients.AddAsync(client);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeleteClient(Client client)
        {
            try
            {
                _db.Clients.Remove(client);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Client> GetClient(int id)
        {
            return await _db.Clients
                .FindAsync(id);
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _db.Clients.ToListAsync();
        }

        public async Task<ClientFavourite> GetFavourite(int clientId, int userId)
        {
            return await _db.Favourites.FirstOrDefaultAsync(f => f.ClientId == clientId && f.UserId == userId);
        }

        public async Task<ClientFavourite> AddFavourite(ClientFavourite clientFav)
        {
            var res = _db.Favourites.Add(clientFav);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeleteFavourite(ClientFavourite clientFav)
        {
            try
            {
                _db.Favourites.Remove(clientFav);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<IEnumerable<Client>> GetClientsByMeal(string mealType)
        {
            return await _db.ClientMealTypes.Where(t => t.MealType.Title.ToUpper().Equals(mealType.ToUpper())).Select(c => c.Client).ToListAsync();
        }

        public async Task<IEnumerable<Client>> GetClientsByName(string title)
        {
            return await _db.Clients.Where(c => c.RestaurantName.ToUpper().Contains(title.ToUpper())).ToListAsync();
        }

        public async Task<Client> UpdateClient(Client client)
        {
            var res = _db.Clients.Update(client);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Client> GetClient(string identityId)
        {
            return await _db.Clients.FirstOrDefaultAsync(c => c.IdentityId == identityId);
        }

        public async Task<ClientImage> GetClientImage(int id)
        {
            return await _db.ClientImages.FindAsync(id);
        }

        public async Task<ClientImage> AddClientImage(ClientImage image)
        {
            var img = _db.ClientImages.Add(image);
            await _db.SaveChangesAsync();
            return img.Entity;
        }

        public async Task AddClientImages(ICollection<ClientImage> images)
        {
            _db.ClientImages.AddRange(images.ToArray());
            await _db.SaveChangesAsync();
        }

        public async Task<bool> DeleteClientImage(ClientImage image)
        {
            try
            {
                _db.ClientImages.Remove(image);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task AddClientClientType(ClientClientType clientClientType)
        {
            await _db.ClientClientTypes.AddAsync(clientClientType);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientMealType(ClientMealType clientMealType)
        {
            await _db.ClientMealTypes.AddAsync(clientMealType);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientCuisine(ClientCuisine clientCuisine)
        {
            await _db.ClientCuisines.AddAsync(clientCuisine);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientDish(ClientDish clientDish)
        {
            await _db.ClientDishes.AddAsync(clientDish);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientGoodFor(ClientGoodFor clientGoodFor)
        {
            await _db.ClientGoodFors.AddAsync(clientGoodFor);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientFeature(ClientFeature clientFeature)
        {
            await _db.ClientFeatures.AddAsync(clientFeature);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientSpecialDiet(ClientSpecialDiet clientSpecialDiet)
        {
            await _db.ClientSpecialDiets.AddAsync(clientSpecialDiet);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientSocialLink(SocialLink socialLink)
        {
            await _db.SocialLinks.AddAsync(socialLink);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientPhone(ClientPhone clientPhone)
        {
            await _db.ClientPhones.AddAsync(clientPhone);

            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientClientType(IEnumerable<ClientClientType> clientClientType)
        {
            _db.ClientClientTypes.RemoveRange(clientClientType);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientMealType(IEnumerable<ClientMealType> clientMealType)
        {
            _db.ClientMealTypes.RemoveRange(clientMealType);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientCuisine(IEnumerable<ClientCuisine> clientCuisine)
        {
            _db.ClientCuisines.RemoveRange(clientCuisine);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientDish(IEnumerable<ClientDish> clientDish)
        {
            _db.ClientDishes.RemoveRange(clientDish);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientGoodFor(IEnumerable<ClientGoodFor> clientGoodFor)
        {
            _db.ClientGoodFors.RemoveRange(clientGoodFor);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientFeature(IEnumerable<ClientFeature> clientFeature)
        {
            _db.ClientFeatures.RemoveRange(clientFeature);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientSpecialDiet(IEnumerable<ClientSpecialDiet> clientSpecialDiet)
        {
            _db.ClientSpecialDiets.RemoveRange(clientSpecialDiet);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveClientSocialLink(IEnumerable<SocialLink> socialLink)
        {
            _db.SocialLinks.RemoveRange(socialLink);
            await _db.SaveChangesAsync();
        }
        public async Task RemoveClientPhone(IEnumerable<ClientPhone> clientPhone)
        {
            _db.ClientPhones.RemoveRange(clientPhone);
            await _db.SaveChangesAsync();
        }



        // Admins

        public async Task<Admin> GetAdmin(string identityId)
        {
            return await _db.Admins.FirstOrDefaultAsync(a => a.IdentityId == identityId);
        }

        public async Task<Admin> GetAdminByIdentityName(string identityName)
        {
            var identityUser = _db.Users.FirstOrDefault(u => u.UserName == identityName);
            if (identityUser != null)
            {
                return await this.GetAdmin(identityUser.Id);
            }
            return null;

        }

        public async Task<ICollection<Admin>> GetAdmins()
        {
            return await _db.Admins.ToListAsync();
        }

        public async Task<Admin> AddAdmin(Admin admin)
        {
            var res = await _db.Admins.AddAsync(admin);
            await _db.SaveChangesAsync();
            return res.Entity;
        }


        // Owners

        public async Task<Owner> AddOwner(Owner owner)
        {
            var res = await _db.Owners.AddAsync(owner);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Owner> GetOwner(string identityId)
        {
            return await _db.Owners.FirstOrDefaultAsync(o => o.IdentityId == identityId);
        }

        public async Task<Owner> GetOwner(int id)
        {
            return await _db.Owners.FindAsync(id);
        }

        public async Task<Owner> UpdateOwner(Owner owner)
        {
            var res = _db.Owners.Update(owner);
            await _db.SaveChangesAsync();
            return res.Entity;
        }


        // Organizations

        public async Task<ICollection<Organization>> GetOrganizations()
        {
            return await _db.Organizations.ToListAsync();
        }

        public async Task<Organization> GetOrganization(int id)
        {
            return await _db.Organizations.FindAsync(id);
        }

        public async Task<Organization> AddOrganization(Organization organization)
        {
            var res = _db.Organizations.Add(organization);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Organization> UpdateOrganization(Organization organization)
        {
            var res = _db.Organizations.Update(organization);
            await _db.SaveChangesAsync();
            return res.Entity;
        }



        // Roles

        public async Task<IdentityRole> GetRole(int id)
        {
            return await _db.Roles.FindAsync(id);
        }

        // Requests

        public async Task<ICollection<ClientRequest>> GetClientRequests()
        {
            return await _db.ClientRequests.ToListAsync();
        }

        public async Task<ICollection<ClientRequest>> GetClientRequests(int take, int skip)
        {
            return await _db.ClientRequests.Skip(skip).Take(take).ToListAsync();
        }

        //public async Task<ICollection<ClientRequest>> GetUndoneClientRequests()
        //{
        //    return await _db.ClientRequests.Where(r => !r.IsDone).ToListAsync();
        //}

        public async Task<ClientRequest> GetClientRequest(int id)
        {
            return await _db.ClientRequests
                .FindAsync(id);
        }

        public async Task UpdateClientRequest(ClientRequest clientRequest)
        {
            _db.ClientRequests.Update(clientRequest);
            await _db.SaveChangesAsync();
        }

        public async Task<ClientRequest> AddClientRequest(ClientRequest clientRequest)
        {
            var res = await _db.ClientRequests.AddAsync(clientRequest);
            await _db.SaveChangesAsync();
            return res.Entity;
        }


        // Tokens

        public async Task<IEnumerable<AccountToken>> GetTokens(string identityId)
        {
            return await _db.AccountTokens.Where(t => t.IdentityUserId == identityId).ToListAsync();
        }


        public async Task<AccountToken> GetToken(string refreshToken)
        {
            return await _db.AccountTokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }

        public async Task<AccountToken> GetTokenByTag(string notificationTag)
        {
            return await _db.AccountTokens.FirstOrDefaultAsync(t => t.NotificationTag == notificationTag);
        }

        public async Task AddToken(AccountToken refreshToken)
        {
            await _db.AccountTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveToken(AccountToken refreshToken)
        {
            _db.AccountTokens.Remove(refreshToken);
            await _db.SaveChangesAsync();
        }


        // Privileges

        public async Task<IEnumerable<UserPrivileges>> GetUserPrivileges(string identityId)
        {
            return await _db.UserPrivileges.Where(p => p.IdentityId.Equals(identityId)).ToListAsync();
        }

        public async Task<UserPrivileges> AddUserPrivilage(UserPrivileges userPrivileges)
        {
            var res = _db.UserPrivileges.Add(userPrivileges);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Privilege> GetPrivilege(int id)
        {
            return await _db.Privileges.FindAsync(id);
        }


        // Parameters        

        public async Task<ICollection<MealType>> GetAllMealTypes()
        {
            return await _db.MealTypes.ToListAsync();
        }

        public async Task<ICollection<Cuisine>> GetAllCuisines()
        {
            return await _db.Cuisines.ToListAsync();
        }

        public async Task<Cuisine> GetCuisine(int id)
        {
            return await _db.Cuisines.FindAsync(id);
        }

        public async Task<Cuisine> GetCuisine(string title)
        {
            return await _db.Cuisines.FirstOrDefaultAsync(c => c.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<Cuisine> AddCuisine(Cuisine cuisine)
        {
            var res = await _db.Cuisines.AddAsync(cuisine);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Cuisine> UpdateCuisine(Cuisine cuisine)
        {
            var res = _db.Cuisines.Update(cuisine);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task DeleteCuisine(Cuisine cuisine)
        {
            _db.Cuisines.Remove(cuisine);
            await _db.SaveChangesAsync();
        }


        public async Task<ICollection<ClientType>> GetAllClientTypes()
        {
            return await _db.ClientTypes.ToListAsync();
        }

        public async Task<ClientType> GetClientType(int id)
        {
            return await _db.ClientTypes.FindAsync(id);
        }

        public async Task<ClientType> GetClientType(string title)
        {
            return await _db.ClientTypes.FirstOrDefaultAsync(t => t.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<ClientType> AddClientType(ClientType clientType)
        {
            var res = await _db.ClientTypes.AddAsync(clientType);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ClientType> UpdateClientType(ClientType clientType)
        {
            var res = _db.ClientTypes.Update(clientType);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task DeleteClientType(ClientType clientType)
        {
            _db.ClientTypes.Remove(clientType);
            await _db.SaveChangesAsync();
        }


        public async Task<ICollection<GoodFor>> GetAllGoodFors()
        {
            return await _db.GoodFors.ToListAsync();
        }

        public async Task<GoodFor> GetGoodFor(int id)
        {
            return await _db.GoodFors.FindAsync(id);
        }

        public async Task<GoodFor> GetGoodFor(string title)
        {
            return await _db.GoodFors.FirstOrDefaultAsync(g => g.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<GoodFor> AddGoodFor(GoodFor goodFor)
        {
            var res = await _db.GoodFors.AddAsync(goodFor);
            await _db.SaveChangesAsync();
            return res.Entity;

        }
        public async Task<GoodFor> UpdateGoodFor(GoodFor goodFor)
        {
            var res = _db.GoodFors.Update(goodFor);
            await _db.SaveChangesAsync();
            return res.Entity;
        }
        public async Task DeleteGoodFor(GoodFor goodFor)
        {
            _db.GoodFors.Remove(goodFor);
            await _db.SaveChangesAsync();
        }


        public async Task<ICollection<Feature>> GetAllFeatures()
        {
            return await _db.Features.ToListAsync();
        }

        public async Task<Feature> GetFeature(int id)
        {
            return await _db.Features.FindAsync(id);
        }

        public async Task<Feature> GetFeature(string title)
        {
            return await _db.Features.FirstOrDefaultAsync(f => f.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<Feature> AddFeature(Feature feature)
        {
            var res = _db.Features.Add(feature);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Feature> UpdateFeature(Feature feature)
        {
            var res = _db.Features.Update(feature);
            await _db.SaveChangesAsync();
            return res.Entity;
        }
        public async Task DeleteFeature(Feature feature)
        {
            _db.Features.Remove(feature);
            await _db.SaveChangesAsync();
        }


        public async Task<ICollection<SpecialDiet>> GetAllSpecialDiets()
        {
            return await _db.SpecialDiets.ToListAsync();
        }

        public async Task<SpecialDiet> GetSpecialDiet(int id)
        {
            return await _db.SpecialDiets.FindAsync(id);
        }

        public async Task<SpecialDiet> GetSpecialDiet(string title)
        {
            return await _db.SpecialDiets.FirstOrDefaultAsync(d => d.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<SpecialDiet> AddSpecialDiet(SpecialDiet specialDiet)
        {
            var res = _db.SpecialDiets.Add(specialDiet);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<SpecialDiet> UpdateSpecialDiet(SpecialDiet specialDiet)
        {
            var res = _db.SpecialDiets.Update(specialDiet);
            await _db.SaveChangesAsync();
            return res.Entity;
        }
        public async Task DeleteSpecialDiet(SpecialDiet specialDiet)
        {
            _db.SpecialDiets.Remove(specialDiet);
            await _db.SaveChangesAsync();
        }

        public async Task<ICollection<Dish>> GetAllDishes()
        {
            return await _db.Dishes.ToListAsync();
        }

        public async Task<Dish> GetDish(int id)
        {
            return await _db.Dishes.FindAsync(id);
        }

        public async Task<Dish> GetDish(string dish)
        {
            return await _db.Dishes.FirstOrDefaultAsync();
        }

        public async Task<Dish> AddDish(Dish dish)
        {
            var res = _db.Dishes.Add(dish);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Dish> UpdateDish(Dish dish)
        {
            var res = _db.Dishes.Update(dish);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task DeleteDish(Dish dish)
        {
            _db.Dishes.Remove(dish);
            await _db.SaveChangesAsync();
        }






        // Users

        public async Task<User> GetUser(int id)
        {
            return await _db.ApplicationUsers.FindAsync(id);
        }

        public async Task<User> GetUser(string identityId)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);
        }

        public async Task<User> AddUser(User user)
        {
            var res = await _db.ApplicationUsers.AddAsync(user);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ICollection<User>> GetUsers()
        {
            return await _db.ApplicationUsers.ToListAsync();
        }

        public async Task<bool> DeleteUser(User user)
        {
            try
            {
                _db.ApplicationUsers.Remove(user);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<User> UpdateUser(User user)
        {
            try
            {

                var res = _db.ApplicationUsers.Update(user);
                await _db.SaveChangesAsync();
                return res.Entity;
            }
            catch
            {
                return null;
            }
        }


        // Waiters


        public async Task<IEnumerable<Waiter>> GetWaiters()
        {
            return await _db.Waiters.ToListAsync();
        }

        public async Task<Waiter> GetWaiter(int id)
        {
            return await _db.Waiters.FindAsync(id);
        }

        public async Task<Waiter> AddWaiter(Waiter waiter)
        {
            var res = _db.Waiters.Add(waiter);
            await _db.SaveChangesAsync();
            return res.Entity;
        }
        public async Task<Waiter> UpdateWaiter(Waiter waiter)
        {
            var res = _db.Waiters.Update(waiter);
            await _db.SaveChangesAsync();
            return res.Entity;
        }
        public async Task<bool> DeleteWaiter(Waiter waiter)
        {
            try
            {
                _db.Waiters.Remove(waiter);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
        public async Task<Waiter> GetWaiter(string identityId)
        {
            return await _db.Waiters.FirstOrDefaultAsync(w => w.IdentityId == identityId);
        }


        //Reservations

        public async Task<Reservation> AddReservation(Reservation reservation)
        {
            var reservationAdded = await _db.Reservations.AddAsync(reservation);
            await _db.SaveChangesAsync();
            return reservationAdded.Entity;
        }

        public async Task<Reservation> UpdateReservation(Reservation reservation)
        {
            var res = _db.Reservations.Update(reservation);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Reservation> GetReservation(int id)
        {
            return await _db.Reservations.FindAsync(id);
        }


        public async Task<ReservationState> GetReservationState(string title)
        {
            return await _db.ReservationStates.FirstOrDefaultAsync(r => r.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<CancelReason> GetCancelReason(int id)
        {
            return await _db.CancelReasons.FindAsync(id);
        }

        public async Task<CancelReason> GetCancelReason(string title)
        {
            return await _db.CancelReasons.FirstOrDefaultAsync(r => r.Title.ToUpper().Equals(title.ToUpper()));
        }


        // Reservation Requests


        public async Task<ReservationRequest> GetReservationRequest(int id)
        {
            return await _db.ReservationRequests.FindAsync(id);
        }

        public async Task<ReservationRequest> AddReservationRequest(ReservationRequest reservationRequest)
        {
            var res = await _db.ReservationRequests.AddAsync(reservationRequest);
            await _db.SaveChangesAsync();
            return res.Entity;
        }



        // Visitors

        public async Task<Visitor> AddVisitor(Visitor visitor)
        {
            var res = _db.Visitors.Add(visitor);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Visitor> GetVisitor(int id)
        {
            return await _db.Visitors.FindAsync(id);
        }

        public async Task<Visitor> UpdateVisitor(Visitor visitor)
        {
            var res = _db.Visitors.Update(visitor);
            await _db.SaveChangesAsync();
            return res.Entity;
        }



        // Tables

        public async Task<Table> GetTable(int id)
        {
            return await _db.Tables.FindAsync(id);
        }


        public async Task<Table> AddTable(Table table)
        {
            var newTable = _db.Tables.Add(table);
            await _db.SaveChangesAsync();
            return newTable.Entity;
        }

        public async Task<Table> UpdateTable(Table table)
        {
            var res = _db.Tables.Update(table);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeleteTable(Table table)
        {
            try
            {
                _db.Tables.Remove(table);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<Bar> GetBar(int id)
        {
            return await _db.Bars.FindAsync(id);
        }

        // Floors

        public async Task<Floor> GetFloor(int clientId, int floorNumber)
        {
            return await _db.Floors.FirstOrDefaultAsync(f => f.ClientId == clientId && f.Number == floorNumber);
        }

        public async Task<Floor> AddFloor(Floor floor)
        {
            var newFloor = _db.Floors.Add(floor);
            await _db.SaveChangesAsync();
            return newFloor.Entity;
        }


        // Halls

        public async Task<Hall> AddHall(Hall hall)
        {
            var newHall = _db.Halls.Add(hall);
            await _db.SaveChangesAsync();
            return newHall.Entity;
        }

        public async Task<Hall> GetHall(int id)
        {
            return await _db.Halls.FindAsync(id);
        }
        public async Task<Hall> UpdateHall(Hall hall)
        {
            var res = _db.Halls.Update(hall);
            await _db.SaveChangesAsync();
            return res.Entity;
        }


        // Events

        public async Task<ICollection<Event>> GetEvents()
        {
            return await _db.Events.ToListAsync();
        }

        public async Task<ICollection<Event>> GetUpcomingEvents()
        {
            return await _db.Events.Where(e => e.Date > DateTime.Now).ToListAsync();
        }

        public async Task<ICollection<Event>> GetEventsByClient(int clientId)
        {
            return await _db.Events.Where(e => e.ClientId == clientId).ToListAsync();
        }

        public async Task<ICollection<Event>> GetUpcomingEventsByClient(int clientId)
        {
            return await _db.Events.Where(e => e.ClientId == clientId && e.Date > DateTime.Now).ToListAsync();
        }

        public async Task<Event> GetEvent(int id)
        {
            return await _db.Events.FindAsync(id);
        }


        public async Task<IEnumerable<Event>> GetEventsByName(string title)
        {
            return await _db.Events.Where(e => e.Title.ToUpper().Contains(title.ToUpper())).ToListAsync();
        }


        public async Task<IEnumerable<Event>> GetEventsByNameAndDescription(string text)
        {
            return await _db.Events.Where(e => e.Title.ToUpper().Contains(text.ToUpper()) || e.Description.ToUpper().Contains(text.ToUpper())).ToListAsync();
        }



        public async Task<Event> AddEvent(Event clientEvent)
        {
            var eventAdded = await _db.Events.AddAsync(clientEvent);
            await _db.SaveChangesAsync();
            return eventAdded.Entity;
        }

        public async Task<Event> UpdateEvent(Event clientEvent)
        {
            var eventUpdated = _db.Events.Update(clientEvent);
            await _db.SaveChangesAsync();
            return eventUpdated.Entity;
        }




        public async Task<EventMark> GetEventMark(int eventId, int userId)
        {
            return await _db.EventMarks.FirstOrDefaultAsync(m => m.EventId == eventId && m.UserId == userId);
        }

        public async Task<EventMark> AddEventMark(EventMark eventMark)
        {
            var res = _db.EventMarks.Add(eventMark);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task RemoveEventMark(EventMark eventMark)
        {
            _db.EventMarks.Remove(eventMark);
            await _db.SaveChangesAsync();
        }


        // Notifications

        public async Task<NotificationType> GetNotificationType(string title)
        {
            return await _db.NotificationTypes.FirstOrDefaultAsync(n => n.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<NotificationType> GetNotificationType(int id)
        {
            return await _db.NotificationTypes.FindAsync(id);
        }

        public async Task<AdminNotification> AddAdminNotification(AdminNotification adminNotification)
        {
            var res = _db.AdminNotifications.Add(adminNotification);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ICollection<AdminNotification>> GetAdminNotifications()
        {
            return await _db.AdminNotifications.ToListAsync();
        }


        public async Task<ICollection<AdminNotification>> GetUndoneAdminNotifications()
        {
            return await _db.AdminNotifications.Where(n => n.Done == null).ToListAsync();
        }

        public async Task<ICollection<AdminNotification>> GetAdminNotifications(int take, int skip)
        {
            return await _db.AdminNotifications.OrderByDescending(n => n.DateTime).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<AdminNotification> GetAdminNotification(int id)
        {
            return await _db.AdminNotifications.FindAsync(id);
        }

        public async Task<AdminNotification> UpdateAdminNotification(AdminNotification adminNotification)
        {
            var res = _db.AdminNotifications.Update(adminNotification);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

    }
}