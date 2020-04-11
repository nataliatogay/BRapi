using BR.Utils.Notification;
using Microsoft.Azure.NotificationHubs;
using System;
using System.Collections.Generic;
using System.Linq;


namespace BR.Services.Interfaces
{
    public interface IPushNotificationService
    {
        HubResponse<NotificationOutcome> SendNotification(string message, MobilePlatform mobilePlatform, string handle, string[] tags = null);

        
    }
}
