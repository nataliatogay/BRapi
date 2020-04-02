﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class CancelReason
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string RoleId { get; set; }
        [ForeignKey("RoleId")]
        public virtual IdentityRole IdentityRole{ get; set; }
        public virtual ICollection<Reservation> Reservations { get; set; }

        public CancelReason()
        {
            Reservations = new HashSet<Reservation>();
        }
    }
}

// cancelled by:
//  1. user
//  2. client
//  3. admin
//     - UserDeleted
//     - ClientDeleted
