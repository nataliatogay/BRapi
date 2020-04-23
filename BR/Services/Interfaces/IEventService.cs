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

        Task<ServerResponse<ICollection<EventInfoShort>>> GetEventsForOwners(int clientId, string ownerIdentityId);

        Task<ServerResponse<ICollection<EventInfoShort>>> GetEventsForClients(string clientIdentityId);

        Task<ServerResponse<EventFullInfo>> GetEventFullInfoForOwners(int eventId, string ownerIdentityId);

        Task<ServerResponse<EventFullInfo>> GetEventFullInfoForClients(int eventId, string clientIdentityId);

        Task<ServerResponse<EventInfoShort>> AddNewEventByOwner(NewEventByOwnerRequest newRequest, string clientIdentityId);

        Task<ServerResponse<EventInfoShort>> AddNewEventByClient(NewEventByClientRequest newRequest, string ownerIdentityId);

        Task<ServerResponse<EventInfoShort>> UpdateEventByOwner(UpdateEventRequest updateRequest, string ownerIdentityId);

        Task<ServerResponse<EventInfoShort>> UpdateEventByClient(UpdateEventRequest updateRequest, string clientIdentityId);

        Task<ServerResponse<string>> UpdateEventImageByOwner(UpdateEventImageRequest updateRequest, string ownerIdentityId);

        Task<ServerResponse<string>> UpdateEventImageByClient(UpdateEventImageRequest updateRequest);




        // ----------------------------------------------------------------------------


        // Task<ICollection<EventInfoShort>> GetAllEventsShortInfo();

        Task<ICollection<EventInfoShort>> GetUpcomingEventsShortInfo();
        
        Task<IEnumerable<Event>> GetEventsByClient(string clientIdentityId);
        
        Task<EventInfo> GetEvent(int id);

        Task<ServerResponse<ICollection<EventInfo>>> GetEventsByName(string name);

        Task<ServerResponse<ICollection<EventInfo>>> GetEventsByNameAndDescription(string name);

        Task<ServerResponse<Event>> AddEvent(NewEventRequest newEventRequest, string addedByUserIdentityId, string role);

        Task<ServerResponse<string>> UpdateEventImage(UpdateEventImageRequest updateRequest);

        Task<ServerResponse> UpdateEvent(UpdateEventRequest updateRequest);

        Task<ServerResponse> AddMark(int eventId, string identityUserId);

        Task<ServerResponse> DeleteMark(int eventId, string identityUserId);
    }
}
