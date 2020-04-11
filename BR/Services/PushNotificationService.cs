using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO.Notifications;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using BR.Utils.Notification;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;

namespace BR.Services
{
    public class PushNotificationService : IPushNotificationService
    {
        private NotificationHubProxy _notificationHubProxy;

       

        public PushNotificationService(IOptions<NotificationHubConfiguration> standardNotificationHubConfiguration)
        {
            _notificationHubProxy = new NotificationHubProxy(standardNotificationHubConfiguration.Value);
        }
        public HubResponse<NotificationOutcome> SendNotification(string message, MobilePlatform mobilePlatform, string handle, string[] tags = null)
        {
            NotificationBody notif = new NotificationBody();
            var messageToSend = "{\"data\":{\"message\":\"" + message + "\"}}";

            notif.Message = messageToSend;
            notif.Platform = mobilePlatform;
            notif.Handle = handle;
            notif.Tags = tags;

            // HubResponse<NotificationOutcome> pushDeliveryResult = _notificationHubProxy.SendNotification(notif).Result;
            return _notificationHubProxy.SendNotification(notif).Result;
        }

        
    }
}
