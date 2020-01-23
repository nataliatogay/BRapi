using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "User")]
    public class ReservationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IReservationService _reservationService;
        private readonly INotificationService _notificationService;

        public ReservationController(UserManager<IdentityUser> userManager,
            IReservationService reservationService,
            INotificationService notificationService)
        {
            _userManager = userManager;
            _reservationService = reservationService;
            _notificationService = notificationService;
        }

        [HttpGet("")]
        public async Task<ActionResult<ICollection<Reservation>>> Get()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return null;
            }
            return new JsonResult(await _reservationService.GetReservations(identityUser.Id));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> Get(int id)
        {
            return new JsonResult(await _reservationService.GetReservation(id));
        }


        [HttpPost("")]
        public async Task<ActionResult<int>> Post([FromBody]NewReservationRequest newReservation)
        {
            // add notification to waiters
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult("User not found");
            }
            var reservation = await _reservationService.AddNewReservation(newReservation, identityUser.Id);
            if(reservation is null)
            {
                return null;
            }
            else
            {
                return reservation.Id;
            }            
        }

        [HttpPut("Complete")]
        public async Task<ActionResult<Reservation>> CompleteReservation(int id)
        {
            return new JsonResult(await _reservationService.CompleteReservation(id));
        }


        [HttpPut("Cancel")]
        public async Task<ActionResult<ServerResponse>> CancelReservation(int id)
        {
            // ok
            return new JsonResult(await _reservationService.CancelReservation(id));
        }

        [HttpPut("ChangeTable")]
        public async Task<ActionResult<Reservation>> ChangeTables(ChangeReservationTablesRequest changeRequest)
        {
            await _reservationService.ChangeTable(changeRequest);
            return Ok();
        }
    }
}