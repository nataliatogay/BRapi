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

        Task<ServerResponse<ICollection<EventInfoShort>>> GetAllEventsForOwners(int clientId, string ownerIdentityId);

        Task<ServerResponse<ICollection<EventInfoShort>>> GetAllEventsForClients(string clientIdentityId);

        Task<ServerResponse<ICollection<EventInfoShort>>> GetUpcomingEventsForOwners(int clientId, string ownerIdentityId);

        Task<ServerResponse<ICollection<EventInfoShort>>> GetUpcomingEventsForClients(string clientIdentityId);

        Task<ServerResponse<ICollection<EventFullInfo>>> GetUpcomingEventsFullInfoForOwners(int clientId, string ownerIdentityId);

        Task<ServerResponse<ICollection<EventFullInfo>>> GetUpcomingEventsFullInfoForClients(string clientIdentityId);

        Task<ServerResponse<EventFullInfo>> GetEventFullInfoForOwners(int eventId, string ownerIdentityId);

        Task<ServerResponse<EventFullInfo>> GetEventFullInfoForClients(int eventId, string clientIdentityId);

        Task<ServerResponse<EventFullInfo>> AddNewEventByOwner(NewEventByOwnerRequest newRequest, string clientIdentityId);

        Task<ServerResponse<EventFullInfo>> AddNewEventByClient(NewEventByClientRequest newRequest, string ownerIdentityId);

        Task<ServerResponse<EventFullInfo>> UpdateEventByOwner(UpdateEventRequest updateRequest, string ownerIdentityId);

        Task<ServerResponse<EventFullInfo>> UpdateEventByClient(UpdateEventRequest updateRequest, string clientIdentityId);

        Task<ServerResponse> PostEventByOwner(int eventId, string ownerIdentityId);

        Task<ServerResponse> PostEventByClient(int eventId, string clientIdentityId);

        Task<ServerResponse> CancelEventByOwner(int eventId, string ownerIdentityId);

        Task<ServerResponse> CancelEventByClient(int eventId, string clientIdentityId);



        // FOR USERS

        Task<ServerResponse<ICollection<int>>> GetUpcomingMarkedEventIds(string userIdentityId);

        Task<ServerResponse<ICollection<EventShortInfoForUsers>>> GetUpcomingMarkedEvents(string userIdentityId, int skip, int take);

        Task<ServerResponse<ICollection<EventShortInfoForUsers>>> GetUpcomingEventsByName(string name, int skip, int take);

        Task<ServerResponse<ICollection<EventShortInfoForUsers>>> GetUpcomingEventsByClient(int clientId, int skip, int take);

        Task<ServerResponse<EventFullInfoForUsers>> GetEventFullInfoForUsers(int eventId);




        // ==============================================================================


        // Task<ICollection<EventInfoShort>> GetAllEventsShortInfo();


        Task<ServerResponse<string>> UpdateEventImageByOwner(UpdateEventImageRequest updateRequest, string ownerIdentityId);

        Task<ServerResponse<string>> UpdateEventImageByClient(UpdateEventImageRequest updateRequest);


        Task<ICollection<EventInfoShort>> GetUpcomingEventsShortInfo();

        Task<IEnumerable<Event>> GetEventsByClient(string clientIdentityId);

        Task<EventInfo> GetEvent(int id);



        Task<ServerResponse<ICollection<EventInfo>>> GetEventsByNameAndDescription(string name);

        Task<ServerResponse<Event>> AddEvent(NewEventRequest newEventRequest, string addedByUserIdentityId, string role);

        Task<ServerResponse<string>> UpdateEventImage(UpdateEventImageRequest updateRequest);

        Task<ServerResponse> UpdateEvent(UpdateEventRequest updateRequest);

        Task<ServerResponse> AddMark(int eventId, string identityUserId);

        Task<ServerResponse> DeleteMark(int eventId, string identityUserId);
    }
}
