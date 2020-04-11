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

        [HttpGet("/all")]
        public async Task<ActionResult<ServerResponse<EventInfoShort>>> GetAll()
        {
            return new JsonResult(Response(await _eventService.GetAllEventsShortInfo()));
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


        // by client, head waiter, owner
        [HttpPost("")]
        public async Task<ActionResult<ServerResponse<Event>>> Post([FromBody]NewEventRequest newEventRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            var roles = await _userManager.GetRolesAsync(identityUser);
            return new JsonResult(await _eventService.AddEvent(newEventRequest, identityUser.Id, roles.FirstOrDefault()));
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