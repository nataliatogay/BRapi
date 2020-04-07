using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }
        
        public string ImagePath { get; set; }

        public bool Gender { get; set; }

        public DateTime? Blocked { get; set; }
        
        public DateTime? Deleted { get; set; }
        
        public DateTime BirthDate { get; set; }

        public int NotificationTime { get; set; } // in mins

        public string IdentityId { get; set; }
        
        public DateTime RegistrationDate { get; set; }

        [ForeignKey("IdentityId")]
        public virtual IdentityUser Identity { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
        
        public virtual ICollection<Invitee> Invitees { get; set; }
        
        public virtual ICollection<UserUserPhone> UserUserPhones { get; set; }
        
        public virtual ICollection<ClientFavourite> ClientFavourites { get; set; }

        public virtual ICollection<ClientRating> ClientRatings { get; set; }

        public virtual ICollection<EventMark> EventMarks { get; set; }


        public User()
        {
            Reservations = new HashSet<Reservation>();
            Invitees = new HashSet<Invitee>();
            UserUserPhones = new HashSet<UserUserPhone>();
            ClientFavourites = new HashSet<ClientFavourite>();
            ClientRatings = new HashSet<ClientRating>();
            EventMarks = new HashSet<EventMark>();
        }

    }
}
