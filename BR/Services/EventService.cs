using BR.DTO;
using BR.DTO.Clients;
using BR.DTO.Events;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using BR.Utils.Notification;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class EventService : IEventService
    {
        private readonly IAsyncRepository _repository;
        private readonly IPushNotificationService _notificationService;
        private readonly IBlobService _blobService;

        public EventService(IAsyncRepository repository,
            IPushNotificationService notificationService,
            IBlobService blobService)
        {
            _repository = repository;
            _notificationService = notificationService;
            _blobService = blobService;
        }




        public async Task<ServerResponse<ICollection<EventInfoShort>>> GetAllEventsForOwners(int clientId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.DbConnectionError, null);
            }

            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.NotFound, null);
            }
            var eventInfos = new List<EventInfoShort>();
            foreach (var item in client.Events)
            {
                eventInfos.Add(this.ToShortEventInfo(item));
            }
            return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.Ok, eventInfos);
        }


        public async Task<ServerResponse<ICollection<EventInfoShort>>> GetAllEventsForClients(string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.UserNotFound, null);
                }

                var eventInfos = new List<EventInfoShort>();
                foreach (var item in client.Events)
                {
                    eventInfos.Add(this.ToShortEventInfo(item));
                }
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.Ok, eventInfos);
            }
            catch
            {
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<ICollection<EventInfoShort>>> GetUpcomingEventsForClients(string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.UserNotFound, null);
                }

                var eventInfos = new List<EventInfoShort>();
                foreach (var item in client.Events)
                {
                    if (item.IsPosted && !item.IsCancelled && item.Date.AddMinutes(item.Duration) > DateTime.Now)
                    {
                        eventInfos.Add(this.ToShortEventInfo(item));
                    }
                }
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.Ok, eventInfos);
            }
            catch
            {
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<ICollection<EventInfoShort>>> GetUpcomingEventsForOwners(int clientId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.DbConnectionError, null);
            }

            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.NotFound, null);
            }

            var eventInfos = new List<EventInfoShort>();
            foreach (var item in client.Events)
            {
                if (item.IsPosted && !item.IsCancelled && item.Date.AddMinutes(item.Duration) > DateTime.Now)
                {
                    eventInfos.Add(this.ToShortEventInfo(item));
                }
            }
            return new ServerResponse<ICollection<EventInfoShort>>(StatusCode.Ok, eventInfos);
        }


        public async Task<ServerResponse<EventFullInfo>> AddNewEventByOwner(NewEventByOwnerRequest newRequest, string ownerIdentityId)
        {
            Owner owner;
            Client client;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                client = owner.Organization.Clients.FirstOrDefault(item => item.Id == newRequest.ClientId);
                if (owner.Organization is null || client is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }

            Event clientEvent = new Event()
            {
                Title = newRequest.Title,
                Description = newRequest.Description,
                Date = DateTime.ParseExact(newRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ClientId = newRequest.ClientId,
                Duration = newRequest.Duration,
                EntranceFee = newRequest.EntranceFee,
                AddedByIdentityId = ownerIdentityId,
                IsPosted = false,
                IsCancelled = false

            };
            string imagePath = null;
            if (!String.IsNullOrEmpty(newRequest.ImageString))
            {
                try
                {
                    imagePath = await _blobService.UploadImage(newRequest.ImageString);
                }
                catch
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.BlobError, null);
                }
            }
            clientEvent.ImagePath = imagePath;
            try
            {
                var newEvent = await _repository.AddEvent(clientEvent);
                return new ServerResponse<EventFullInfo>(StatusCode.Ok, this.ToFullEventInfo(newEvent));
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.Error, null);
            }
        }


        public async Task<ServerResponse<ICollection<EventFullInfo>>> GetUpcomingEventsFullInfoForOwners(int clientId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<EventFullInfo>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<EventFullInfo>>(StatusCode.DbConnectionError, null);
            }

            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<EventFullInfo>>(StatusCode.NotFound, null);
            }

            var eventInfos = new List<EventFullInfo>();
            foreach (var item in client.Events)
            {
                if (item.Date.AddMinutes(item.Duration) > DateTime.Now && !item.IsCancelled)
                {
                    eventInfos.Add(this.ToFullEventInfo(item));
                }
            }
            return new ServerResponse<ICollection<EventFullInfo>>(StatusCode.Ok, eventInfos);
        }


        public async Task<ServerResponse<ICollection<EventFullInfo>>> GetUpcomingEventsFullInfoForClients(string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<EventFullInfo>>(StatusCode.UserNotFound, null);
                }

                var eventInfos = new List<EventFullInfo>();
                foreach (var item in client.Events)
                {
                    if (item.Date.AddMinutes(item.Duration) > DateTime.Now && !item.IsCancelled)
                    {
                        eventInfos.Add(this.ToFullEventInfo(item));
                    }
                }
                return new ServerResponse<ICollection<EventFullInfo>>(StatusCode.Ok, eventInfos);
            }
            catch
            {
                return new ServerResponse<ICollection<EventFullInfo>>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<EventFullInfo>> AddNewEventByClient(NewEventByClientRequest newRequest, string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }

            Event clientEvent = new Event()
            {
                Title = newRequest.Title,
                Description = newRequest.Description,
                Date = DateTime.ParseExact(newRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ClientId = client.Id,
                Duration = newRequest.Duration,
                EntranceFee = newRequest.EntranceFee,
                AddedByIdentityId = clientIdentityId,
                IsPosted = false,
                IsCancelled = false

            };
            string imagePath = null;
            if (!String.IsNullOrEmpty(newRequest.ImageString))
            {
                try
                {
                    imagePath = await _blobService.UploadImage(newRequest.ImageString);
                }
                catch
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.BlobError, null);
                }
            }
            clientEvent.ImagePath = imagePath;
            try
            {
                var newEvent = await _repository.AddEvent(clientEvent);
                return new ServerResponse<EventFullInfo>(StatusCode.Ok, this.ToFullEventInfo(newEvent));
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.Error, null);
            }
        }


        public async Task<ServerResponse<EventFullInfo>> GetEventFullInfoForOwners(int eventId, string ownerIdentityId)
        {

            Owner owner;
            Event clientEvent;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (owner is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (owner.Organization is null
                    || owner.Organization.Clients is null
                    || clientEvent is null
                    || !owner.Organization.Clients.Contains(clientEvent.Client))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
                return new ServerResponse<EventFullInfo>(StatusCode.Ok,
                    this.ToFullEventInfo(clientEvent));

            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<EventFullInfo>> GetEventFullInfoForClients(int eventId, string clientIdentityId)
        {

            Client client;
            Event clientEvent;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (client is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (!client.Events.Contains(clientEvent))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
                return new ServerResponse<EventFullInfo>(StatusCode.Ok,
                    this.ToFullEventInfo(clientEvent));

            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<EventFullInfo>> UpdateEventByOwner(UpdateEventRequest updateRequest, string ownerIdentityId)
        {
            Owner owner;
            Event clientEvent;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                clientEvent = await _repository.GetEvent(updateRequest.EventId);
                if (owner is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (owner.Organization is null
                    || owner.Organization.Clients is null
                    || clientEvent is null
                    || !owner.Organization.Clients.Contains(clientEvent.Client))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }

            clientEvent.Title = updateRequest.Title.Trim();
            clientEvent.EntranceFee = updateRequest.EntranceFee;
            clientEvent.Duration = updateRequest.Duration;
            clientEvent.Description = updateRequest.Description.Trim();
            clientEvent.Date = DateTime.ParseExact(updateRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (updateRequest.ImageString != null)
            {
                string path;
                try
                {
                    path = await _blobService.UploadImage(updateRequest.ImageString);

                }
                catch
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.BlobError, null);
                }
                if (clientEvent.ImagePath != null)
                {
                    try
                    {
                        await _blobService.DeleteImage(clientEvent.ImagePath);
                    }
                    catch { }
                }


                clientEvent.ImagePath = path;
            }
            try
            {
                clientEvent = await _repository.UpdateEvent(clientEvent);
                return new ServerResponse<EventFullInfo>(StatusCode.Ok, this.ToFullEventInfo(clientEvent));
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<EventFullInfo>> UpdateEventByClient(UpdateEventRequest updateRequest, string clientIdentityId)
        {
            Client client;
            Event clientEvent;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                clientEvent = await _repository.GetEvent(updateRequest.EventId);
                if (client is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (clientEvent is null
                    || !client.Events.Contains(clientEvent))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }

            clientEvent.Title = updateRequest.Title.Trim();
            clientEvent.EntranceFee = updateRequest.EntranceFee;
            clientEvent.Duration = updateRequest.Duration;
            clientEvent.Description = updateRequest.Description.Trim();
            clientEvent.Date = DateTime.ParseExact(updateRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            if (updateRequest.ImageString != null)
            {
                string path;
                try
                {
                    path = await _blobService.UploadImage(updateRequest.ImageString);

                }
                catch
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.BlobError, null);
                }
                if (clientEvent.ImagePath != null)
                {
                    try
                    {
                        await _blobService.DeleteImage(clientEvent.ImagePath);
                    }
                    catch { }
                }

                clientEvent.ImagePath = path;
            }

            try
            {
                clientEvent = await _repository.UpdateEvent(clientEvent);
                return new ServerResponse<EventFullInfo>(StatusCode.Ok, this.ToFullEventInfo(clientEvent));
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }
        }



        public async Task<ServerResponse> PostEventByOwner(int eventId, string ownerIdentityId)
        {
            Owner owner;
            Event clientEvent;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (owner is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (owner.Organization is null
                    || owner.Organization.Clients is null
                    || clientEvent is null
                    || !owner.Organization.Clients.Contains(clientEvent.Client))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }
            if (clientEvent.Date < DateTime.Now)
            {
                return new ServerResponse(StatusCode.Expired);
            }
            clientEvent.IsPosted = true;
            try
            {
                await _repository.UpdateEvent(clientEvent);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }


        public async Task<ServerResponse> PostEventByClient(int eventId, string clientIdentityId)
        {
            Client client;
            Event clientEvent;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (client is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (clientEvent is null
                    || !client.Events.Contains(clientEvent))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }
            if (clientEvent.Date < DateTime.Now)
            {
                return new ServerResponse(StatusCode.Expired);
            }
            clientEvent.IsPosted = true;
            try
            {
                await _repository.UpdateEvent(clientEvent);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }


        public async Task<ServerResponse> CancelEventByOwner(int eventId, string ownerIdentityId)
        {
            Owner owner;
            Event clientEvent;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (owner is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (owner.Organization is null
                    || owner.Organization.Clients is null
                    || clientEvent is null
                    || !owner.Organization.Clients.Contains(clientEvent.Client))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }

            if (!clientEvent.IsPosted)
            {
                try
                {
                    await _repository.RemoveEvent(clientEvent);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }
            }

            clientEvent.IsCancelled = true;
            try
            {
                await _repository.UpdateEvent(clientEvent);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            // notify users

            var userTags = new List<string>();
            try
            {

                foreach (var item in clientEvent.EventMarks)
                {
                    var tokens = await _repository.GetTokens(item.User.IdentityId);
                    foreach (var token in tokens)
                    {
                        userTags.Add(token.NotificationTag);
                    }
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            try
            {
                _notificationService.SendNotification("Expired", MobilePlatform.gcm, "string", userTags.ToArray());
            }
            catch { }
            return new ServerResponse(StatusCode.Ok);

        }


        public async Task<ServerResponse> CancelEventByClient(int eventId, string clientIdentityId)
        {
            Client client;
            Event clientEvent;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (client is null)
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.UserNotFound, null);
                }
                if (clientEvent is null
                    || !client.Events.Contains(clientEvent))
                {
                    return new ServerResponse<EventFullInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventFullInfo>(StatusCode.DbConnectionError, null);
            }

            if (!clientEvent.IsPosted)
            {
                try
                {
                    await _repository.RemoveEvent(clientEvent);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }
            }

            clientEvent.IsCancelled = true;
            try
            {
                await _repository.UpdateEvent(clientEvent);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            // notify users

            var userTags = new List<string>();
            try
            {

                foreach (var item in clientEvent.EventMarks)
                {
                    var tokens = await _repository.GetTokens(item.User.IdentityId);
                    foreach (var token in tokens)
                    {
                        userTags.Add(token.NotificationTag);
                    }
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            try
            {
                _notificationService.SendNotification("Expired", MobilePlatform.gcm, "string", userTags.ToArray());
            }
            catch { }
            return new ServerResponse(StatusCode.Ok);

        }




        // FOR USERS

        public async Task<ServerResponse<ICollection<int>>> GetUpcomingMarkedEventIds(string userIdentityId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(userIdentityId);
                if (user is null)
                {
                    return new ServerResponse<ICollection<int>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<int>>(StatusCode.DbConnectionError, null);
            }

            var eventIds = new List<int>();
            foreach (var item in user.EventMarks)
            {
                if (item.Event.Date > DateTime.Now)
                {
                    eventIds.Add(item.EventId);
                }
            }
            return new ServerResponse<ICollection<int>>(StatusCode.Ok, eventIds);
        }


        public async Task<ServerResponse<EventShortInfoForUsersResponse>> GetUpcomingMarkedEvents(string userIdentityId, int skip, int take)
        {
            User user;
            try
            {
                user = await _repository.GetUser(userIdentityId);
                if (user is null)
                {
                    return new ServerResponse<EventShortInfoForUsersResponse>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventShortInfoForUsersResponse>(StatusCode.DbConnectionError, null);
            }

            var res = new List<EventShortInfoForUsers>();
            var events = user.EventMarks.Where(item => item.Event.Date.AddMinutes(item.Event.Duration) > DateTime.Now);
            int count = events.Count();
            events = events.Skip(skip).Take(take);
            foreach (var item in events)
            {
                res.Add(this.EventToEventShortInfoForUsers(item.Event));
            }
            return new ServerResponse<EventShortInfoForUsersResponse>(StatusCode.Ok,
                new EventShortInfoForUsersResponse()
                {
                    TotalCount = count,
                    Events = res
                });
        }


        public async Task<ServerResponse<EventShortInfoForUsersResponse>> GetUpcomingEventsByName(string name, int skip, int take)
        {

            IEnumerable<Event> events;
            int count;
            try
            {
                events = await _repository.GetUpcomingEventsByName(name, skip, take);
                count = await _repository.GetUpcomingEventsByNameCount(name);
            }
            catch
            {
                return new ServerResponse<EventShortInfoForUsersResponse>(StatusCode.DbConnectionError, null);
            }

            var res = new List<EventShortInfoForUsers>();
            foreach (var item in events)
            {
                res.Add(this.EventToEventShortInfoForUsers(item));
            }
            return new ServerResponse<EventShortInfoForUsersResponse>(StatusCode.Ok,
                new EventShortInfoForUsersResponse()
                {
                    TotalCount = count,
                    Events = res
                });
        }


        public async Task<ServerResponse<EventShortInfoForUsersResponse>> GetUpcomingEventsByClient(int clientId, int skip, int take)
        {
            IEnumerable<Event> events;
            int count;
            try
            {
                events = await _repository.GetUpcomingEventsByClient(clientId, skip, take);
                count = await _repository.GetUpcomingEventsByClientCount(clientId);
            }
            catch
            {
                return new ServerResponse<EventShortInfoForUsersResponse>(StatusCode.DbConnectionError, null);
            }

            var res = new List<EventShortInfoForUsers>();
            foreach (var item in events)
            {
                res.Add(this.EventToEventShortInfoForUsers(item));
            }
            return new ServerResponse<EventShortInfoForUsersResponse>(StatusCode.Ok,
                new EventShortInfoForUsersResponse()
                {
                    TotalCount = count,
                    Events = res
                });

        }


        public async Task<ServerResponse<EventFullInfoForUsers>> GetEventFullInfoForUsers(int eventId)
        {
            Event clientEvent;
            try
            {
                clientEvent = await _repository.GetEvent(eventId);
                if (clientEvent is null)
                {
                    return new ServerResponse<EventFullInfoForUsers>(StatusCode.NotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<EventFullInfoForUsers>(StatusCode.DbConnectionError, null);
            }

            var phones = new List<ClientPhoneInfo>();
            foreach (var item in clientEvent.Client.ClientPhones)
            {
                phones.Add(new ClientPhoneInfo()
                {
                    Number = item.Number,
                    IsWhatsApp = item.IsWhatsApp,
                    IsTelegram = item.IsTelegram
                });
            }

            var users = new List<EventMarkedUser>();
            foreach (var item in clientEvent.EventMarks)
            {
                users.Add(new EventMarkedUser()
                {
                    UserId = item.UserId,
                    UserImagePath = item.User.ImagePath
                });
            }

            return new ServerResponse<EventFullInfoForUsers>(StatusCode.Ok, new EventFullInfoForUsers()
            {
                Id = clientEvent.Id,
                ClientId = clientEvent.ClientId,
                ClientName = clientEvent.Client.RestaurantName,
                DateTime = clientEvent.Date,
                Description = clientEvent.Description,
                Duration = clientEvent.Duration,
                EntranceFee = clientEvent.EntranceFee,
                ImagePath = clientEvent.ImagePath is null ? clientEvent.Client.LogoPath : clientEvent.ImagePath,
                EventMarkCount = clientEvent.EventMarks.Count(),
                Title = clientEvent.Title,
                Phones = phones,
                Users = users
            });

        }

        public async Task<ServerResponse> AddMark(int eventId, string identityUserId)
        {
            User user;
            try
            {

                user = await _repository.GetUser(identityUserId);
                if (user is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
                var clientEvent = await _repository.GetEvent(eventId);
                if (clientEvent is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
                if (clientEvent.Date < DateTime.Now)
                {
                    return new ServerResponse(StatusCode.Expired);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            var eventMark = new EventMark()
            {
                EventId = eventId,
                UserId = user.Id
            };

            try
            {
                var res = await _repository.AddEventMark(eventMark);
                return new ServerResponse(StatusCode.Ok);
            }
            catch (DbUpdateException)
            {
                return new ServerResponse(StatusCode.Duplicate);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }

        }

        public async Task<ServerResponse> DeleteMark(int eventId, string identityUserId)
        {
            User user;
            EventMark eventMark;
            try
            {

                user = await _repository.GetUser(identityUserId);
                if (user is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
                eventMark = await _repository.GetEventMark(eventId, user.Id);
                if (eventMark is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }


            if (eventMark.Event.Date < DateTime.Now)
            {
                return new ServerResponse(StatusCode.Expired);
            }

            try
            {
                await _repository.RemoveEventMark(eventMark);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }




        private EventShortInfoForUsers EventToEventShortInfoForUsers(Event clientEvent)
        {
            if (clientEvent is null)
            {
                return null;
            }

            return new EventShortInfoForUsers()
            {
                Id = clientEvent.Id,
                DateTime = clientEvent.Date,
                Description = clientEvent.Description,
                EntranceFee = clientEvent.EntranceFee,
                ImagePath = clientEvent.ImagePath is null ? clientEvent.Client.LogoPath : clientEvent.ImagePath,
                Title = clientEvent.Title
            };
        }

        private EventFullInfo ToFullEventInfo(Event clientEvent)
        {
            if (clientEvent is null)
            {
                return null;
            }
            return new EventFullInfo()
            {
                Id = clientEvent.Id,
                Date = clientEvent.Date,
                ImagePath = clientEvent.ImagePath is null ? clientEvent.Client.LogoPath : clientEvent.ImagePath,
                Title = clientEvent.Title,
                Description = clientEvent.Description,
                Duration = clientEvent.Duration,
                EntranceFee = clientEvent.EntranceFee,
                IsPosted = clientEvent.IsPosted,
                IsCancelled = clientEvent.IsCancelled,
                MarkCount = clientEvent.EventMarks.Count()
            };
        }


        private EventInfoShort ToShortEventInfo(Event clientEvent)
        {
            if (clientEvent is null)
            {
                return null;
            }
            return new EventInfoShort()
            {
                Id = clientEvent.Id,
                StartDateTime = clientEvent.Date,
                EndDateTime = clientEvent.Date.AddMinutes(clientEvent.Duration),
                ImagePath = clientEvent.ImagePath is null ? clientEvent.Client.LogoPath : clientEvent.ImagePath,
                Title = clientEvent.Title,
                MarkCount = clientEvent.EventMarks.Count(),
                IsCancelled = clientEvent.IsCancelled,
                IsPosted = clientEvent.IsPosted,
                EntranceFee = clientEvent.EntranceFee
            };
        }






        // ===================================================================================





        public async Task<ServerResponse<string>> UpdateEventImageByOwner(UpdateEventImageRequest updateRequest, string ownerIdentityId)
        {
            Owner owner;
            Event clientEvent;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<string>(StatusCode.UserNotFound, null);
                }
                clientEvent = await _repository.GetEvent(updateRequest.EventId);
                if (clientEvent is null || owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(clientEvent.Client))
                {
                    return new ServerResponse<string>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }

            string path;
            try
            {
                path = await _blobService.UploadImage(updateRequest.ImageString);

            }
            catch
            {
                return new ServerResponse<string>(StatusCode.BlobError, null);
            }
            if (clientEvent.ImagePath != null)
            {
                try
                {
                    await _blobService.DeleteImage(clientEvent.ImagePath);
                }
                catch { }
            }

            clientEvent.ImagePath = path;
            try
            {
                await _repository.UpdateEvent(clientEvent);
                return new ServerResponse<string>(StatusCode.Ok, path);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<string>> UpdateEventImageByClient(UpdateEventImageRequest updateRequest)
        {
            Event clientEvent;
            try
            {
                clientEvent = await _repository.GetEvent(updateRequest.EventId);
                if (clientEvent is null)
                {
                    return new ServerResponse<string>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }

            string path;
            try
            {
                path = await _blobService.UploadImage(updateRequest.ImageString);

            }
            catch
            {
                return new ServerResponse<string>(StatusCode.BlobError, null);
            }
            if (clientEvent.ImagePath != null)
            {
                try
                {
                    await _blobService.DeleteImage(clientEvent.ImagePath);
                }
                catch { }
            }

            clientEvent.ImagePath = path;
            try
            {
                await _repository.UpdateEvent(clientEvent);
                return new ServerResponse<string>(StatusCode.Ok, path);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
        }



        public async Task<ICollection<EventInfoShort>> GetUpcomingEventsShortInfo()
        {
            var events = await _repository.GetUpcomingEvents();
            if (events is null)
            {
                return null;
            }
            var res = new List<EventInfoShort>();
            foreach (var item in events)
            {
                res.Add(this.ToShortEventInfo(item));
            }
            return res;

        }

        public async Task<IEnumerable<Event>> GetEventsByClient(string clientIdentityId)
        {
            var client = await _repository.GetClient(clientIdentityId);
            if (client is null)
            {
                return null;
            }
            return await _repository.GetEventsByClient(client.Id);
        }

        public async Task<EventInfo> GetEvent(int id)
        {
            var clientEvent = await _repository.GetEvent(id);
            return this.ToEventInfo(clientEvent);
        }




        public async Task<ServerResponse<ICollection<EventInfo>>> GetEventsByNameAndDescription(string name)
        {
            try
            {
                var events = await _repository.GetEventsByNameAndDescription(name);
                var res = new List<EventInfo>();
                foreach (var item in events)
                {
                    res.Add(this.ToEventInfo(item));
                }
                return new ServerResponse<ICollection<EventInfo>>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<ICollection<EventInfo>>(StatusCode.DbConnectionError, null);
            }
        }








        private EventInfo ToEventInfo(Event clientEvent)
        {
            if (clientEvent is null)
            {
                return null;
            }
            return new EventInfo()
            {
                Id = clientEvent.Id,
                Date = clientEvent.Date,
                Description = clientEvent.Description,
                ImgPath = clientEvent.ImagePath is null ? clientEvent.Client.LogoPath : clientEvent.ImagePath,
                Title = clientEvent.Title,
                ClientId = clientEvent.Client.Id,
                ClientName = clientEvent.Client.RestaurantName
            };
        }
    }
}
