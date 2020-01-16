using BR.Utils.Notification;
using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface INotificationService
    {
        HubResponse<NotificationOutcome> SendNotification(string message, MobilePlatform mobilePlatform, string handle, string[] tags = null);
    }
}
