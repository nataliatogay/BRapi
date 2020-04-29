﻿using System;
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
        private readonly IPushNotificationService _notificationService;

        public ReservationController(UserManager<IdentityUser> userManager,
            IReservationService reservationService,
            IPushNotificationService notificationService)
        {
            _userManager = userManager;
            _reservationService = reservationService;
            _notificationService = notificationService;
        }



        [HttpPost("pending")]
        public async Task<ActionResult<ServerResponse<ServerResponse<string>>>> SetPendingState(TableState stateRequest)
        {
            return new JsonResult(await _reservationService.SetPendingTableState(stateRequest));
        }


        // by user
        [HttpPost("newReservation")]
        public async Task<ActionResult<ServerResponse>> NewReservation([FromBody]NewReservationByUserRequest newReservation)
        {
            // add notification to waiters
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(_reservationService.AddNewReservationByUser(newReservation, identityUser.Id));
        }



        //=========================================================================


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

        

        [HttpPost("BarPending")]
        public async Task<ActionResult<ServerResponse<ServerResponse<string>>>> SetBarPendingState(BarStates stateRequest)
        {
            return new JsonResult(await _reservationService.SetBarPendingTableState(stateRequest));
        }


        

        [HttpPost("bar")]
        public async Task<ActionResult<ServerResponse<int>>> NewBarReservation([FromBody]NewBarReservationRequest newBarReservation)
        {
            // add notification to waiters
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            var result = await _reservationService.AddNewBarReservation(newBarReservation, identityUser.Id);
            if (result.Data is null)
            {
                return new JsonResult(Response(result.StatusCode));
            }
            else
            {
                return new JsonResult(Response(result.StatusCode, result.Data.Id));
            }
        }




        [HttpPost("Confirm")]
        public async Task<ActionResult<ServerResponse>> ConfirmReservation([FromBody]ConfirmReservationRequest confirmRequest)
        {
            return new JsonResult(await _reservationService.AddConfirmedReservation(confirmRequest));
        }


        [HttpPost("BarConfirm")]
        public async Task<ActionResult<ServerResponse>> BarConfirmReservation([FromBody]ConfirmBarReservationRequest confirmRequest)
        {
            return new JsonResult(await _reservationService.AddBarConfirmedReservation(confirmRequest));
        }

        [HttpGet("TableStates")]
        public async Task<ActionResult<ServerResponse<ICollection<TableCurrentStateCacheData>>>> GetTableStates(TableState tableStatesRequests)
        {
            return new JsonResult(await _reservationService.GetTablesStates(tableStatesRequests));
        }


        [HttpGet("BarStates")]
        public async Task<ActionResult<ServerResponse<ICollection<TableCurrentStateCacheData>>>> GetBarStates(BarStates barStatesRequests)
        {
            return new JsonResult(await _reservationService.GetBarStates(barStatesRequests));
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


        [HttpPost("BarByPhone")]
        public async Task<ActionResult<ServerResponse>> NewBarReservationByPhone(NewBarReservationByPhoneRequest reservationRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _reservationService.AddBarReservationByPhone(reservationRequest));
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
        public async Task<ActionResult<ServerResponse>> CancelReservation([FromBody]CancelReservationRequest cancelRequest)
        {

            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _reservationService.CancelReservation(cancelRequest.ReservationId, cancelRequest.CancelReasonId, identityUser.Id));

        }

        [HttpPut("ChangeTable")]
        public async Task<ActionResult<Reservation>> ChangeTables([FromBody]ChangeReservationTablesRequest changeRequest)
        {
            try
            {
                return new JsonResult(await _reservationService.ChangeTable(changeRequest));
            }
            catch
            {
                return new JsonResult(Response(Utils.StatusCode.Error));
            }
        }



        // by client, head waiter
        [HttpPost("Visitor")]
        public async Task<ActionResult<ServerResponse>> NewVisitor([FromBody]NewVisitRequest visitRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _reservationService.AddNewVisitor(visitRequest, identityUser.Id));
        }


        [HttpPost("CompleteVisit")]
        public async Task<ActionResult<ServerResponse>> CompleteVisit([FromBody]int visitId)
        {

            return new JsonResult(await _reservationService.CompleteVisit(visitId));
        }
    }
}