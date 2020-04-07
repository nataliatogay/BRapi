using BR.DTO;
using BR.DTO.Events;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IEventService
    {
        Task<ICollection<EventInfoShort>> GetAllEventsShortInfo();
        
        Task<ICollection<EventInfoShort>> GetUpcomingEventsShortInfo();
        
        Task<IEnumerable<Event>> GetEventsByClient(string clientIdentityId);
        
        Task<EventInfo> GetEvent(int id);
     
        Task<ServerResponse<Event>> AddEvent(NewEventRequest newEventRequest, string addedByUserIdentityId, string role);

        Task<ServerResponse<string>> UpdateEventImage(UpdateEventImageRequest updateRequest);

        Task<ServerResponse> UpdateEvent(UpdateEventRequest updateRequest);

        Task<ServerResponse> AddMark(int eventId, string identityUserId);

        Task<ServerResponse> DeleteMark(int eventId, string identityUserId);
    }
}
