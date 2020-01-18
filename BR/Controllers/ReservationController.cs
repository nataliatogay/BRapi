using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Services;
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

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]NewReservationRequest newReservation)
        {
            // add notification to waiters
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult("User not found");
            }
            await _reservationService.AddNewReservation(newReservation, identityUser.Id);
            return Ok(); // reservation id
        }
    }
}