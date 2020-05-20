using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Events;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ResponseController
    {
        private readonly IEventService _eventService;
        private readonly UserManager<IdentityUser> _userManager;

        public EventController(IEventService eventService,
            UserManager<IdentityUser> userManager)
        {
            _eventService = eventService;
            _userManager = userManager;
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("ShortForOwners/{clientId}")]
        public async Task<ActionResult<ServerResponse<ICollection<EventInfoShort>>>> GetAllEventsForOwners(int clientId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<EventInfoShort>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetAllEventsForOwners(clientId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("ShortForClients")]
        public async Task<ActionResult<ServerResponse<ICollection<EventInfoShort>>>> GetAllEventsForClients()
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<EventInfoShort>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetAllEventsForClients(clientIdentityUser.Id));
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("UpcomingForOwner/{clientId}")]
        public async Task<ActionResult<ServerResponse<ICollection<EventInfoShort>>>> GetUpcomingEventsForOwners(int clientId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<EventInfoShort>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetUpcomingEventsForOwners(clientId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("UpcomingForClients")]
        public async Task<ActionResult<ServerResponse<ICollection<EventInfoShort>>>> GetUpcomingEventsForClients()
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<EventInfoShort>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetUpcomingEventsForClients(clientIdentityUser.Id));
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("UpcomingFullInfoForOwner/{clientId}")]
        public async Task<ActionResult<ServerResponse<ICollection<EventFullInfo>>>> GetUpcomingEventsFullInfoForOwners(int clientId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<EventFullInfo>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetUpcomingEventsFullInfoForOwners(clientId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("UpcomingFullInfoForClients")]
        public async Task<ActionResult<ServerResponse<ICollection<EventFullInfo>>>> GetUpcomingEventsFullInfoForClients()
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<EventFullInfo>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetUpcomingEventsFullInfoForClients(clientIdentityUser.Id));
        }



        [Authorize(Roles = "Owner")]
        [HttpGet("FullForOwners/{eventId}")]
        public async Task<ActionResult<ServerResponse<EventFullInfo>>> GetEventForOwners(int eventId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventFullInfo>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetEventFullInfoForOwners(eventId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("FullForClients/{eventId}")]
        public async Task<ActionResult<ServerResponse<EventFullInfo>>> GetEventForClients(int eventId)
        {
            var clientrIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientrIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventFullInfo>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetEventFullInfoForClients(eventId, clientrIdentityUser.Id));
        }


        [Authorize(Roles = "Owner")]
        [HttpPost("NewByOwner")]
        public async Task<ActionResult<ServerResponse<EventFullInfo>>> AddNewEventByOwner([FromBody]NewEventByOwnerRequest newEventRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.AddNewEventByOwner(newEventRequest, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpPost("NewByClient")]
        public async Task<ActionResult<ServerResponse<EventFullInfo>>> AddNewEventByClient([FromBody]NewEventByClientRequest newEventRequest)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.AddNewEventByClient(newEventRequest, clientIdentityUser.Id));
        }


        [Authorize(Roles = "Owner")]
        [HttpPut("UpdateByOwner")]
        public async Task<ActionResult<ServerResponse<EventFullInfo>>> UpdateByOwner([FromBody]UpdateEventRequest updateRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.UpdateEventByOwner(updateRequest, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpPut("UpdateByClient")]
        public async Task<ActionResult<ServerResponse<EventFullInfo>>> UpdateByClient([FromBody]UpdateEventRequest updateRequest)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.UpdateEventByClient(updateRequest, clientIdentityUser.Id));
        }



        [Authorize(Roles = "Owner")]
        [HttpPut("PostByOwner")]
        public async Task<ActionResult<ServerResponse>> PostEventByOwner([FromBody]int eventId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _eventService.PostEventByOwner(eventId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpPut("PostByClient")]
        public async Task<ActionResult<ServerResponse>> PostEventByClient([FromBody]int eventId)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _eventService.PostEventByClient(eventId, clientIdentityUser.Id));
        }


        [Authorize(Roles = "Owner")]
        [HttpPut("CancelByOwner")]
        public async Task<ActionResult<ServerResponse>> CancelEventByOwner([FromBody] int eventId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _eventService.CancelEventByOwner(eventId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpPut("CancelByClient")]
        public async Task<ActionResult<ServerResponse>> CancelEventByClient([FromBody] int eventId)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _eventService.CancelEventByClient(eventId, clientIdentityUser.Id));
        }




        // FOR USERS

        [Authorize(Roles = "User")]
        [HttpGet("UpcomingMarkedEventIds")]
        public async Task<ActionResult<ServerResponse<ICollection<int>>>> GetUpcomingMarkedEventIds()
        {
            var userIdentity = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userIdentity is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetUpcomingMarkedEventIds(userIdentity.Id));
        }


        [Authorize(Roles = "User")]
        [HttpGet("UpcomingMarkedEvents")]
        public async Task<ActionResult<ServerResponse<ICollection<EventShortInfoForUsers>>>> GetUpcomingMarkedEvents(int skip, int take)
        {
            var userIdentity = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userIdentity is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.GetUpcomingMarkedEvents(userIdentity.Id, skip, take));
        }


        [Authorize(Roles = "User")]
        [HttpGet("UpcomingByName")]
        public async Task<ActionResult<ServerResponse<ICollection<EventShortInfoForUsers>>>> GetUpcomingEventsByName(string name, int skip, int take)
        {
            return new JsonResult(await _eventService.GetUpcomingEventsByName(name, skip, take));
        }


        [Authorize(Roles = "User")]
        [HttpGet("UpcomingByClient/{clientId}")]
        public async Task<ActionResult<ServerResponse<ICollection<EventShortInfoForUsers>>>> GetUpcomingEventsByClient(int clientId, int skip, int take)
        {
            return new JsonResult(await _eventService.GetUpcomingEventsByClient(clientId, skip, take));
        }


        [Authorize(Roles = "User")]
        [HttpGet("FullInfoForUsers/{eventId}")]
        public async Task<ActionResult<ServerResponse<EventFullInfoForUsers>>> GetFullEventInfoForUsers(int eventId)
        {
            return new JsonResult(await _eventService.GetEventFullInfoForUsers(eventId));
        }



        // ============================================================================================





        [Authorize(Roles = "Owner")]
        [HttpPut("UpdateImageByOwner")]
        public async Task<ActionResult<ServerResponse<string>>> UpdateImageByOwner([FromBody]UpdateEventImageRequest updateRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.UpdateEventImageByOwner(updateRequest, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpPut("UpdateImageByClient")]
        public async Task<ActionResult<ServerResponse<string>>> UpdateImageByClientOwner([FromBody]UpdateEventImageRequest updateRequest)
        {
            return new JsonResult(await _eventService.UpdateEventImageByClient(updateRequest));
        }


        [HttpGet("Upcoming")]
        public async Task<ActionResult<ServerResponse<ICollection<EventInfo>>>> GetUpcoming()
        {
            return new JsonResult(Response(await _eventService.GetUpcomingEventsShortInfo()));
        }





        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<EventInfo>>> Get(int id)
        {
            return new JsonResult(await _eventService.GetEvent(id));
        }






        [HttpPut("")]
        public async Task<ActionResult<Cuisine>> Put([FromBody]UpdateEventRequest updateRequest)
        {
            return new JsonResult(await _eventService.UpdateEvent(updateRequest));
        }

        [HttpPost("UpdateImage")]
        public async Task<ActionResult<string>> UploadImage([FromBody]UpdateEventImageRequest updateRequest)
        {

            var path = await _eventService.UpdateEventImage(updateRequest);
            return new JsonResult(path);
        }


        // role = user
        [HttpPost("Mark")]
        public async Task<ActionResult<ServerResponse>> AddMark(int eventId)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _eventService.AddMark(eventId, identityUser.Id));
        }

        [HttpDelete("Mark")]
        public async Task<ActionResult<ServerResponse>> DeleteFavourite(int eventId)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _eventService.DeleteMark(eventId, identityUser.Id));

        }




        [HttpGet("DescriptionSearch")]
        public async Task<ActionResult<ServerResponse<IEnumerable<EventInfo>>>> DescriptionSearch(string text)
        {
            return new JsonResult(Response(await _eventService.GetEventsByNameAndDescription(text)));
        }

    }
}