using BR.DTO;
using BR.DTO.Events;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
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

        public async Task<ICollection<EventInfoShort>> GetAllEventsShortInfo()
        {
            var events = await _repository.GetEvents();
            if(events is null)
            {
                return null;
            }
            var res = new List<EventInfoShort>();
            foreach(var item in events)
            {
                res.Add(this.ToShortEventInfo(item));
            }
            return res;
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

        public async Task<Event> AddEvent(NewEventRequest newEventRequest, string clientIdentityId)
        {
            //var client = await _repository.GetClient(clientIdentityId);
            //if (client is null)
            //{
            //    return null;
            //}
            Event clientEvent = new Event()
            {
                Title = newEventRequest.Title,
                Description = newEventRequest.Description,
                Date = DateTime.ParseExact(newEventRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                //ClientId = client.Id,
                ClientId=3,
                EventTypeId = newEventRequest.EventTypeId
            };
            string imagePath;
            if (!String.IsNullOrEmpty(newEventRequest.Image))
            {
                imagePath = await _blobService.UploadImage(newEventRequest.Image);
            }
            else
            {
                imagePath = "https://rb2020storage.blob.core.windows.net/photos/default-event.png";
            }
            clientEvent.ImagePath = imagePath;
            return await _repository.AddEvent(clientEvent);
        }

        public async Task<string> UpdateEventImage(UpdateEventImageRequest updateRequest)
        {
            var clientEvent = await _repository.GetEvent(updateRequest.EventId);
            if (clientEvent is null)
            {
                return null;
            }
            var path = await _blobService.UploadImage(updateRequest.ImageString);
            await _blobService.DeleteImage(clientEvent.ImagePath);
            clientEvent.ImagePath = path;
            await _repository.UpdateEvent(clientEvent);
            return path;
        }

        public async Task<Event> UpdateEvent(UpdateEventRequest updateRequest)
        {
            var clientEvent = await _repository.GetEvent(updateRequest.EventId);
            if (clientEvent is null)
            {
                return null;
            }
            clientEvent.Date = DateTime.ParseExact(updateRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            clientEvent.Description = updateRequest.Description;
            clientEvent.Title = updateRequest.Title;
            return await _repository.UpdateEvent(clientEvent);
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
                Title = clientEvent.Title
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
                Type = clientEvent.EventType.Title,
                ClientId = clientEvent.Client.Id,
                ClientName = clientEvent.Client.RestaurantName
            };
        }
    }
}
