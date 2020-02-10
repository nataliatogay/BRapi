using BR.DTO;
using BR.DTO.Events;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IEventService
    {
        Task<IEnumerable<Event>> GetAllEvents();
        Task<ICollection<EventInfoShort>> GetUpcomingEventsShortInfo();
        Task<IEnumerable<Event>> GetEventsByClient(string clientIdentityId);
        Task<EventInfo> GetEvent(int id);
        Task<Event> AddEvent(NewEventRequest newEventRequest, string clientIdentityId);
        Task<Event> UpdateEvent(UpdateEventRequest updateRequest);
        Task<string> UpdateEventImage(UpdateEventImageRequest updateRequest);
    }
}
