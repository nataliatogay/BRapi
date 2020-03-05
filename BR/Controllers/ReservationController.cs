using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Reservations;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using BR.DTO.Redis;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "User")]
    public class ReservationController : ResponseController
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

        [HttpPost("pending")]
        public async Task<ActionResult<ServerResponse<ServerResponse>>> SetPendingState(TableStatesRequests stateRequest)
        {
            return new JsonResult(await _reservationService.SetPendingTableState(stateRequest));
        }


        [HttpPost("")]
        public async Task<ActionResult<ServerResponse<int>>> Post([FromBody]NewReservationRequest newReservation)
        {
            // add notification to waiters
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            var result = await _reservationService.AddNewReservation(newReservation, identityUser.Id);
            if (result.Data is null)
            {
                return new JsonResult(Response(result.StatusCode));
            }
            else
            {
                return new JsonResult(Response(result.StatusCode, result.Data));
            }
        }



        [HttpPost("Confirm")]
        public async Task<ActionResult<ServerResponse>> ConfirmReservation([FromBody]ConfirmReservationRequest confirmRequest)
        {
            return new JsonResult(await _reservationService.AddConfirmedReservation(confirmRequest));
        }

        [HttpGet("TableStates")]
        public async Task<ActionResult<ServerResponse<ICollection<TableCurrentStateCacheData>>>> GetTableStates(TableStatesRequests tableStatesRequests)
        {
            return new JsonResult(await _reservationService.GetTablesStates(tableStatesRequests));
        }

        [HttpPost("ByPhone")]
        public async Task<ActionResult<ServerResponse>> NewReservationByPhone(NewReservationByPhoneRequest reservationRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _reservationService.AddReservationByPhone(reservationRequest, identityUser.Id));
        }


        [HttpPut("Complete")]
        public async Task<ActionResult<Reservation>> CompleteReservation(int id)
        {
            try
            {
                var res = await _reservationService.CompleteReservation(id);
                if (res != null)
                {
                    return new JsonResult(Response(Utils.StatusCode.Ok));
                }
            }
            catch
            {
                return new JsonResult(Response(Utils.StatusCode.Error));
            }
            return new JsonResult(Response(Utils.StatusCode.Error));
        }


        [HttpPut("Cancel")]
        public async Task<ActionResult<ServerResponse>> CancelReservation(int id)
        {
            try
            {
                var res = await _reservationService.CancelReservation(id);
                if (res != null)
                {
                    return new JsonResult(Response(Utils.StatusCode.Ok));
                }
            }
            catch { return new JsonResult(Response(Utils.StatusCode.Error)); }
            return new JsonResult(Response(Utils.StatusCode.Error));
        }

        [HttpPut("ChangeTable")]
        public async Task<ActionResult<Reservation>> ChangeTables([FromBody]ChangeReservationTablesRequest changeRequest)
        {
            try
            {
                await _reservationService.ChangeTable(changeRequest);
                return new JsonResult(Response(Utils.StatusCode.Ok));
            }
            catch
            {
                return new JsonResult(Response(Utils.StatusCode.Error));
            }
        }
    }
}