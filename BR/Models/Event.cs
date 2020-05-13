
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Event
    {
        public int Id { get; set; }
        
        public DateTime Date { get; set; }

        public int Duration { get; set; }  // in mins

        [Required]
        public string Title { get; set; }
        
        public string Description { get; set; }

        public int EntranceFee { get; set; }

        [Required]
        public string ImagePath { get; set; }
        
        public int ClientId { get; set; }

        public bool IsPosted { get; set; }

        public bool IsCancelled { get; set; }

        public virtual Client Client { get; set; }

        public string AddedByIdentityId { get; set; }

        [ForeignKey("AddedByIdentityId")]
        public virtual IdentityUser AddedByIdentity { get; set; }

        public virtual ICollection<EventMark> EventMarks { get; set; }

        public Event()
        {
            EventMarks = new HashSet<EventMark>();
        }
    }
}
