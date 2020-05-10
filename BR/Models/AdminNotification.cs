using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class AdminNotification
    {
        public int Id { get; set; }
        
        public DateTime DateTime { get; set; }

        [Required]
        public string Title { get; set; }

        public int NotificationTypeId { get; set; }

        public bool? Done { get; set; }

        public int? OwnerRequestId { get; set; }

        public int? ClientId { get; set; }

        [ForeignKey("NotificationTypeId")]
        public virtual NotificationType NotificationType { get; set; }

        [ForeignKey("OwnerRequestId")]
        public virtual OwnerRequest OwnerRequest { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }


    }
}
