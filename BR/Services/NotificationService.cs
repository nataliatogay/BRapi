using BR.DTO.Notifications;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class NotificationService :INotificationService
    {
        private readonly IAsyncRepository _repository;

        public NotificationService(IAsyncRepository repository)
        {
            _repository = repository;
        }


        public async Task<ServerResponse<ICollection<AdminNotificationInfo>>> GetAdminNotifications(int take, int skip)
        {
            ICollection<AdminNotification> notifications;
            try
            {
                notifications = await _repository.GetAdminNotifications(take, skip);

                ICollection<AdminNotificationInfo> res = new List<AdminNotificationInfo>();
                if (notifications != null)
                {
                    foreach (var item in notifications)
                    {
                        res.Add(new AdminNotificationInfo()
                        {
                            Id = item.Id,
                            DateTime = item.DateTime,
                            Done = item.Done,
                            NotificationType = item.NotificationType.Title,
                            Title = item.Title,
                            ClientId = item.ClientId,
                            RequestId = item.OwnerRequestId
                        });
                    }
                }

                return new ServerResponse<ICollection<AdminNotificationInfo>>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<ICollection<AdminNotificationInfo>>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse<int>> GetAdminNotificationCount()
        {
            ICollection<AdminNotification> notifications;
            try
            {
                notifications = await _repository.GetAdminNotifications();

                if(notifications is null)
                {
                    return new ServerResponse<int>(StatusCode.Ok, 0);
                }

                return new ServerResponse<int>(StatusCode.Ok, notifications.Count());
            }
            catch
            {
                return new ServerResponse<int>(StatusCode.DbConnectionError, 0);
            }
        }

        public async Task<ServerResponse<int>> GetUndoneAdminNotificationCount()
        {
            ICollection<AdminNotification> notifications;
            try
            {
                notifications = await _repository.GetUndoneAdminNotifications();

                if (notifications is null)
                {
                    return new ServerResponse<int>(StatusCode.Ok, 0);
                }

                return new ServerResponse<int>(StatusCode.Ok, notifications.Count());
            }
            catch
            {
                return new ServerResponse<int>(StatusCode.DbConnectionError, 0);
            }
        }
    }
}
