using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO.Notifications;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ResponseController
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("AdminNotifications")]
        public async Task<ActionResult<ServerResponse<ICollection<AdminNotificationInfo>>>> GetAdminNotifications(int take, int skip)
        {
            return new JsonResult(await _notificationService.GetAdminNotifications(take, skip));
        }

        [HttpGet("AdminNotificationsCount")]
        public async Task<ActionResult<ServerResponse<ICollection<AdminNotificationInfo>>>> GetAdminNotificationsCount()
        {
            return new JsonResult(await _notificationService.GetAdminNotificationCount());
        }


        [HttpGet("UndoneAdminNotificationsCount")]
        public async Task<ActionResult<ServerResponse<ICollection<AdminNotificationInfo>>>> GetUndoneAdminNotificationsCount()
        {
            return new JsonResult(await _notificationService.GetUndoneAdminNotificationCount());
        }
    }
}