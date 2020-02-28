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

        //[HttpGet("")]
        //public async Task<ActionResult<ServerResponse<EventInfo>>> Get()
        //{
        //    var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
        //    if (identityUser is null)
        //    {
        //        return new JsonResult("Client not found");
        //    }
        //    return new JsonResult(await _eventService.GetEventsByClient(identityUser.Id));
        //}

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

        [HttpPost("")]
        public async Task<ActionResult<Event>> Post([FromBody]NewEventRequest newEventRequest)
        {
            //var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //if (identityUser is null)
            //{
            //    return new JsonResult("Client not found");
            //}
            //var clientEvent = await _eventService.AddEvent(newEventRequest, identityUser.Id);
            var clientEvent = await _eventService.AddEvent(newEventRequest, "123");
            return new JsonResult(clientEvent);
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

    }
}