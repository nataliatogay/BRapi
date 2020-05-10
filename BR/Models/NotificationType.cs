using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class NotificationType
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<AdminNotification> AdminNotifications { get; set; }

        public NotificationType()
        {
            AdminNotifications = new HashSet<AdminNotification>();
        }
    }
}


// request
// registration
// update
// delete
