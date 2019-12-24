using BR.EF;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz;
using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BR.Services;
using BR.Controllers;
using BR.Utils.Notification;
using Microsoft.Azure.NotificationHubs;
using Microsoft.Extensions.Options;
using NotificationBody = BR.Utils.Notification.NotificationBody;

namespace BR.Utils
{
    [DisallowConcurrentExecution]
    public class DailyReservationReminderJob : IJob
    {
        private readonly IServiceProvider _provider;
        private readonly ILogger<DailyReservationReminderJob> _logger;
        private NotificationHubProxy _notificationHubProxy;
        static int counter = 0;
        public DailyReservationReminderJob(IServiceProvider provider, 
            ILogger<DailyReservationReminderJob> logger, IOptions<NotificationHubConfiguration> standardNotificationHubConfiguration)
        {
            _provider = provider;
            _logger = logger;

            _notificationHubProxy = new NotificationHubProxy(standardNotificationHubConfiguration.Value);
        }

        public Task Execute(IJobExecutionContext context)
        {
            using (var scope = _provider.CreateScope())
            {
                NotificationBody notif = new NotificationBody();

                notif.Message = "{\"data\":{\"message\":\"Notification" + counter++ + "\"}}";
                notif.Platform = MobilePlatform.gcm;
                notif.Handle = "string";
                notif.Tags = null;

                HubResponse<NotificationOutcome> pushDeliveryResult = _notificationHubProxy.SendNotification(notif).Result;

                //var dbContext = scope.ServiceProvider.GetService<BRDbContext>();
                //var userService  = scope.ServiceProvider.GetService<IWaiterService>();
                //var waiter = userService.GetWaiter(1).Result;
                //_logger.LogInformation(waiter.FirstName);
                // fetch customers, send email, update DB
            }

            return Task.CompletedTask;
        }
    }
}
