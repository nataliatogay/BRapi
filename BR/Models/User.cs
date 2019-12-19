﻿using Microsoft.AspNetCore.Identity;
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

        // [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

       // [MaxLength(50)]
       [Required]
        public string LastName { get; set; }

        public string ImagePath { get; set; }

        public bool? Gender { get; set; }

        public bool IsBlocked { get; set; }

       // public string Email { get; set; } -> хранится в Identity
        public DateTime? BirthDate { get; set; }
        
        public string IdentityId { get; set; }

        [ForeignKey("IdentityId")]
        public virtual IdentityUser Identity { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<Invitee> Invitees { get; set; }
        public virtual ICollection<AccountToken> AccountTokens { get; set; }
        //public virtual ICollection<UserMail> UserMails { get; set; }

        public User()
        {
            Reservations = new HashSet<Reservation>();
            Invitees = new HashSet<Invitee>();
            AccountTokens = new HashSet<AccountToken>();
            // UserMails = new HashSet<UserMail>();
        }

    }
}
