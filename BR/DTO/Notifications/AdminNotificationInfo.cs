using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Notifications
{
    public class AdminNotificationInfo
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public DateTime DateTime { get; set; }

        public bool? Done { get; set; }

        public string NotificationType { get; set; }

        public int? RequestId { get; set; }

        public int? ClientId { get; set; }

    }
}
