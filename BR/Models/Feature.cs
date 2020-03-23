using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Feature
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public bool Editable { get; set; }
    }
}


// Not editable:

// WiFi
// Children Zone
// Parking
// Live Music
// Business Lunch
// Smoking Area
// Cards Accepted
// Bar Reservation

