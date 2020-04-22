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
        public async Task<ActionResult<ServerResponse<ICollection<EventInfoShort>>>> GetEventsForOwners(int clientId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _eventService.GetEventsForOwners(clientId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("ShortForClients")]
        public async Task<ActionResult<ServerResponse<ICollection<EventInfoShort>>>> GetEventsForClients()
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _eventService.GetEventsForClients(clientIdentityUser.Id));
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("FullForOwners/{eventId}")]
        public async Task<ActionResult<ServerResponse<EventFullInfoForOwners>>> GetEventForOwners(int eventId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _eventService.GetEventFullInfoForOwners(eventId, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("FullForClients/{eventId}")]
        public async Task<ActionResult<ServerResponse<EventFullInfoForOwners>>> GetEventForClients(int eventId)
        {
            var clientrIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientrIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _eventService.GetEventFullInfoForClients(eventId, clientrIdentityUser.Id));
        }


        [Authorize(Roles = "Owner")]
        [HttpPost("NewByOwner")]
        public async Task<ActionResult<ServerResponse<EventInfoShort>>> AddNewEventByOwner([FromBody]NewEventByOwnerRequest newEventRequest)
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
        public async Task<ActionResult<ServerResponse<EventInfoShort>>> AddNewEventByClient([FromBody]NewEventByClientRequest newEventRequest)
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
        public async Task<ActionResult<ServerResponse<EventInfoShort>>> UpdateByOwner ([FromBody]UpdateEventRequest updateRequest)
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
        public async Task<ActionResult<ServerResponse<EventInfoShort>>> UpdateByClient([FromBody]UpdateEventRequest updateRequest)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<EventInfoShort>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _eventService.UpdateEventByClient(updateRequest, clientIdentityUser.Id));
        }




        // ----------------------------------------------------------------------------



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



        [HttpGet("Search")]
        public async Task<ActionResult<ServerResponse<IEnumerable<EventInfo>>>> Search(string name)
        {
            return new JsonResult(Response(await _eventService.GetEventsByName(name)));
        }


        [HttpGet("DescriptionSearch")]
        public async Task<ActionResult<ServerResponse<IEnumerable<EventInfo>>>> DescriptionSearch(string text)
        {
            return new JsonResult(Response(await _eventService.GetEventsByNameAndDescription(text)));
        }

    }
}