using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Visitor
    {
        public int Id { get; set; }

        public int? TableId { get; set; }

        public int? BarTableId { get; set; }

        public DateTime StartDateTime { get; set; }

        public int Duration { get; set; }

        public int GuestCount { get; set; }

        public string AddedByIdentityId { get; set; }

        public bool IsCompleted { get; set; }

        [ForeignKey("TableId")]
        public virtual Table Table { get; set; }

        [ForeignKey("BarTableId")]
        public virtual BarTable BarTable { get; set; }

        [ForeignKey("AddedByIdentityId")]
        public virtual IdentityUser AddedByIdentity { get; set; }
    }
}
