using BR.DTO;
using BR.DTO.Events;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
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
        private readonly IBlobService _blobService;

        public EventService(IAsyncRepository repository,
            IBlobService blobService)
        {
            _repository = repository;
            _blobService = blobService;
        }




        public async Task<ServerResponse<ICollection<EventInfoShort>>> GetEventsForOwners(int clientId, string ownerIdentityId)
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


        public async Task<ServerResponse<ICollection<EventInfoShort>>> GetEventsForClients(string clientIdentityId)
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



        public async Task<ServerResponse<EventInfoShort>> AddNewEventByOwner(NewEventByOwnerRequest newRequest, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.UserNotFound, null);
                }
                if (owner.Organization is null || owner.Organization.Clients.FirstOrDefault(item => item.Id == newRequest.ClientId) is null)
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.DbConnectionError, null);
            }

            Event clientEvent = new Event()
            {
                Title = newRequest.Title,
                Description = newRequest.Description,
                Date = DateTime.ParseExact(newRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ClientId = newRequest.ClientId,
                Duration = newRequest.Duration,
                EntranceFee = newRequest.EntranceFee,
                AddedByIdentityId = ownerIdentityId

            };
            string imagePath;
            if (!String.IsNullOrEmpty(newRequest.Image))
            {
                try
                {

                    imagePath = await _blobService.UploadImage(newRequest.Image);
                }
                catch
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.BlobError, null);
                }
            }
            else
            {
                imagePath = "https://rb2020storage.blob.core.windows.net/photos/default-event.png";
            }
            clientEvent.ImagePath = imagePath;
            try
            {
                var newEvent = await _repository.AddEvent(clientEvent);
                return new ServerResponse<EventInfoShort>(StatusCode.Ok, this.ToShortEventInfo(newEvent));
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.Error, null);
            }
        }


        public async Task<ServerResponse<EventInfoShort>> AddNewEventByClient(NewEventByClientRequest newRequest, string clientIdentityId)
        {
            Client client;
            try
            {
                client= await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.DbConnectionError, null);
            }

            Event clientEvent = new Event()
            {
                Title = newRequest.Title,
                Description = newRequest.Description,
                Date = DateTime.ParseExact(newRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ClientId = client.Id,
                Duration = newRequest.Duration,
                EntranceFee = newRequest.EntranceFee,
                AddedByIdentityId = clientIdentityId

            };
            string imagePath;
            if (!String.IsNullOrEmpty(newRequest.Image))
            {
                try
                {

                    imagePath = await _blobService.UploadImage(newRequest.Image);
                }
                catch
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.BlobError, null);
                }
            }
            else
            {
                imagePath = "https://rb2020storage.blob.core.windows.net/photos/default-event.png";
            }
            clientEvent.ImagePath = imagePath;
            try
            {
                var newEvent = await _repository.AddEvent(clientEvent);
                return new ServerResponse<EventInfoShort>(StatusCode.Ok, this.ToShortEventInfo(newEvent));
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.Error, null);
            }
        }


        public async Task<ServerResponse<EventFullInfoForOwners>> GetEventFullInfoForOwners(int eventId, string ownerIdentityId)
        {

            Owner owner;
            Event clientEvent;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (owner is null)
                {
                    return new ServerResponse<EventFullInfoForOwners>(StatusCode.UserNotFound, null);
                }
                if (owner.Organization is null
                    || owner.Organization.Clients is null
                    || clientEvent is null
                    || !owner.Organization.Clients.Contains(clientEvent.Client))
                {
                    return new ServerResponse<EventFullInfoForOwners>(StatusCode.NotFound, null);
                }
                return new ServerResponse<EventFullInfoForOwners>(StatusCode.Ok,
                    new EventFullInfoForOwners()
                    {
                        Id = clientEvent.Id,
                        Date = clientEvent.Date,
                        Description = clientEvent.Description,
                        Duration = clientEvent.Duration,
                        EntranceFee = clientEvent.EntranceFee,
                        ImagePath = clientEvent.ImagePath,
                        Title = clientEvent.Title,
                        MarkCount = clientEvent.EventMarks.Count()
                    });

            }
            catch
            {
                return new ServerResponse<EventFullInfoForOwners>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<EventFullInfoForOwners>> GetEventFullInfoForClients(int eventId, string clientIdentityId)
        {

            Client client;
            Event clientEvent;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                clientEvent = await _repository.GetEvent(eventId);
                if (client is null)
                {
                    return new ServerResponse<EventFullInfoForOwners>(StatusCode.UserNotFound, null);
                }
                if (!client.Events.Contains(clientEvent))
                {
                    return new ServerResponse<EventFullInfoForOwners>(StatusCode.NotFound, null);
                }
                return new ServerResponse<EventFullInfoForOwners>(StatusCode.Ok,
                    new EventFullInfoForOwners()
                    {
                        Id = clientEvent.Id,
                        Date = clientEvent.Date,
                        Description = clientEvent.Description,
                        Duration = clientEvent.Duration,
                        EntranceFee = clientEvent.EntranceFee,
                        ImagePath = clientEvent.ImagePath,
                        Title = clientEvent.Title,
                        MarkCount = clientEvent.EventMarks.Count
                    });

            }
            catch
            {
                return new ServerResponse<EventFullInfoForOwners>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<EventInfoShort>> UpdateEventByOwner(UpdateEventRequest updateRequest, string ownerIdentityId)
        {
            Owner owner;
            Event clientEvent;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                clientEvent = await _repository.GetEvent(updateRequest.EventId);
                if (owner is null)
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.UserNotFound, null);
                }
                if (owner.Organization is null
                    || owner.Organization.Clients is null
                    || clientEvent is null
                    || !owner.Organization.Clients.Contains(clientEvent.Client))
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.DbConnectionError, null);
            }

            clientEvent.Title = updateRequest.Title;
            clientEvent.EntranceFee = updateRequest.EntranceFee;
            clientEvent.Duration = updateRequest.Duration;
            clientEvent.Description = updateRequest.Description;
            clientEvent.Date = DateTime.ParseExact(updateRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            try
            {
                clientEvent = await _repository.UpdateEvent(clientEvent);
                return new ServerResponse<EventInfoShort>(StatusCode.Ok, this.ToShortEventInfo(clientEvent));
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<EventInfoShort>> UpdateEventByClient(UpdateEventRequest updateRequest, string clientIdentityId)
        {
            Client client;
            Event clientEvent;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                clientEvent = await _repository.GetEvent(updateRequest.EventId);
                if (client is null)
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.UserNotFound, null);
                }
                if (clientEvent is null
                    || !client.Events.Contains(clientEvent))
                {
                    return new ServerResponse<EventInfoShort>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.DbConnectionError, null);
            }

            clientEvent.Title = updateRequest.Title;
            clientEvent.EntranceFee = updateRequest.EntranceFee;
            clientEvent.Duration = updateRequest.Duration;
            clientEvent.Description = updateRequest.Description;
            clientEvent.Date = DateTime.ParseExact(updateRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            try
            {
                clientEvent = await _repository.UpdateEvent(clientEvent);
                return new ServerResponse<EventInfoShort>(StatusCode.Ok, this.ToShortEventInfo(clientEvent));
            }
            catch
            {
                return new ServerResponse<EventInfoShort>(StatusCode.DbConnectionError, null);
            }
        }






        // ----------------------------------------------------------------------------





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


        public async Task<ServerResponse<ICollection<EventInfo>>> GetEventsByName(string name)
        {
            try
            {
                var events = await _repository.GetEventsByName(name);
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


        public async Task<ServerResponse<Event>> AddEvent(NewEventRequest newEventRequest, string addedByUserIdentityId, string role)
        {
            //if (role is null)
            //{
            //    return new ServerResponse<Event>(StatusCode.RoleNotFound, null);
            //}
            //if (role.Equals("OWNER") && newEventRequest.ClientId is null)
            //{
            //    return new ServerResponse<Event>(StatusCode.Error, null);
            //}
            //try
            //{
            //    if (role.Equals("WAITER"))
            //    {
            //        var waiter = await _repository.GetWaiter(addedByUserIdentityId);
            //        if (waiter is null)
            //        {
            //            return new ServerResponse<Event>(StatusCode.UserNotFound, null);
            //        }
            //        newEventRequest.ClientId = waiter.ClientId;
            //    }
            //    else if (role.Equals("CLIENT"))
            //    {
            //        var client = await _repository.GetClient(addedByUserIdentityId);
            //        if (client is null)
            //        {
            //            return new ServerResponse<Event>(StatusCode.UserNotFound, null);
            //        }
            //        newEventRequest.ClientId = client.Id;
            //    }
            //}
            //catch
            //{
            //    return new ServerResponse<Event>(StatusCode.DbConnectionError, null);
            //}


            Event clientEvent = new Event()
            {
                Title = newEventRequest.Title,
                Description = newEventRequest.Description,
                Date = DateTime.ParseExact(newEventRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                //ClientId = client.Id,
                ClientId = newEventRequest.ClientId ?? default,
                Duration = newEventRequest.Duration,
                EntranceFee = newEventRequest.EntranceFee,
                //AddedByIdentityId = addedByUserIdentityId
                AddedByIdentityId = "ghj"

            };
            string imagePath;
            if (!String.IsNullOrEmpty(newEventRequest.Image))
            {
                try
                {

                    imagePath = await _blobService.UploadImage(newEventRequest.Image);
                }
                catch
                {
                    return new ServerResponse<Event>(StatusCode.BlobError, null);
                }
            }
            else
            {
                imagePath = "https://rb2020storage.blob.core.windows.net/photos/default-event.png";
            }
            clientEvent.ImagePath = imagePath;
            try
            {
                var newEvent = await _repository.AddEvent(clientEvent);
                return new ServerResponse<Event>(StatusCode.Ok, newEvent);
            }
            catch
            {
                return new ServerResponse<Event>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<string>> UpdateEventImage(UpdateEventImageRequest updateRequest)
        {
            Event clientEvent;
            try
            {

                clientEvent = await _repository.GetEvent(updateRequest.EventId);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            if (clientEvent is null)
            {
                return new ServerResponse<string>(StatusCode.NotFound, null);
            }
            string path;
            try
            {
                path = await _blobService.UploadImage(updateRequest.ImageString);

                await _blobService.DeleteImage(clientEvent.ImagePath);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.BlobError, null);
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

        public async Task<ServerResponse> UpdateEvent(UpdateEventRequest updateRequest)
        {
            Event clientEvent;
            try
            {
                clientEvent = await _repository.GetEvent(updateRequest.EventId);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (clientEvent is null)
            {
                return new ServerResponse(StatusCode.NotFound);
            }
            clientEvent.Date = DateTime.ParseExact(updateRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            clientEvent.Description = updateRequest.Description.Trim();
            clientEvent.Title = updateRequest.Title.Trim();
            clientEvent.Duration = updateRequest.Duration;
            clientEvent.EntranceFee = updateRequest.EntranceFee;
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




        private EventInfoShort ToShortEventInfo(Event clientEvent)
        {
            if (clientEvent is null)
            {
                return null;
            }
            return new EventInfoShort()
            {
                Id = clientEvent.Id,
                Date = clientEvent.Date,
                ImagePath = clientEvent.ImagePath,
                Title = clientEvent.Title,
                MarkCount = clientEvent.EventMarks.Count()
            };
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
                ImgPath = clientEvent.ImagePath,
                Title = clientEvent.Title,
                ClientId = clientEvent.Client.Id,
                ClientName = clientEvent.Client.RestaurantName
            };
        }
    }
}
