using BR.DTO.Notifications;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface INotificationService
    {
        Task<ServerResponse<ICollection<AdminNotificationInfo>>> GetAdminNotifications(int take, int skip);

        Task<ServerResponse<int>> GetAdminNotificationCount();

        Task<ServerResponse<int>> GetUndoneAdminNotificationCount();
    }
}
