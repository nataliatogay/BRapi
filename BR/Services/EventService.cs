using BR.DTO;
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

        public async Task<IEnumerable<Event>> GetAllEvents()
        {
            return await _repository.GetEvents();
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

        public async Task<Event> GetEvent(int id)
        {
            return await _repository.GetEvent(id);
        }

        public async Task<Event> AddEvent(NewEventRequest newEventRequest, string clientIdentityId)
        {
            var client = await _repository.GetClient(clientIdentityId);
            if (client is null)
            {
                return null;
            }
            Event clientEvent = new Event()
            {
                Title = newEventRequest.Title,
                Description = newEventRequest.Description,
                Date = DateTime.ParseExact(newEventRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ClientId = client.Id
            };
            var imagePath = await _blobService.UploadImage(newEventRequest.Image);
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
            if(clientEvent is null)
            {
                return null;
            }
            clientEvent.Date = DateTime.ParseExact(updateRequest.Date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            clientEvent.Description = updateRequest.Description;
            clientEvent.Title = updateRequest.Title;
            return await _repository.UpdateEvent(clientEvent);
        }
    }
}
