using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class Client
    {
        public int Id { get; set; }

        [Required]
        public string RestaurantName { get; set; }

        public float Lat { get; set; }

        public float Long { get; set; }

        public int OpenTime { get; set; } //  the number of minutes past midnight

        public int CloseTime { get; set; } //  the number of minutes past midnight

        public int PriceCategory { get; set; }

        public int MaxReserveDays { get; set; }

        public int ReserveDurationAvg { get; set; } // in mins

        public int? BarReserveDurationAvg { get; set; } // in mins

        public int ConfirmationDuration { get; set; } // in mins

        public string Description { get; set; }

        public DateTime RegistrationDate { get; set; }

        public bool IsConfirmedByAdmin { get; set; }

        public DateTime? Blocked { get; set; }

        public DateTime? Deleted { get; set; }

        [Required]
        public string MainImagePath { get; set; }

        public string IdentityId { get; set; }

        public int OrganizationId { get; set; }

        [ForeignKey("IdentityId")]
        public virtual IdentityUser Identity { get; set; }

        [ForeignKey("OrganizationId")]
        public virtual Organization Organization { get; set; }

        public virtual ICollection<SocialLink> SocialLinks { get; set; }

        public virtual ICollection<ClientPhone> ClientPhones { get; set; }

        public virtual ICollection<Floor> Floors { get; set; }

        public virtual ICollection<ClientMealType> ClientMealTypes { get; set; }

        public virtual ICollection<ClientClientType> ClientClientTypes { get; set; }

        public virtual ICollection<ClientCuisine> ClientCuisines { get; set; }

        public virtual ICollection<ClientSpecialDiet> ClientSpecialDiets { get; set; }

        public virtual ICollection<ClientGoodFor> ClientGoodFors { get; set; }

        public virtual ICollection<ClientFeature> ClientFeatures { get; set; }

        public virtual ICollection<ClientDish> ClientDishes { get; set; }

        public virtual ICollection<News> News { get; set; }

        public virtual ICollection<Waiter> Waiters { get; set; }

        public virtual ICollection<Event> Events { get; set; }

        public virtual ICollection<ClientImage> ClientImages { get; set; }

        public virtual ICollection<ClientFavourite> ClientFavourites { get; set; }

        public virtual ICollection<ClientRating> ClientRatings { get; set; }

        public virtual ICollection<Reservation> Reservations { get; set; }



        public Client()
        {
            SocialLinks = new HashSet<SocialLink>();
            ClientPhones = new HashSet<ClientPhone>();
            Floors = new HashSet<Floor>();
            ClientMealTypes = new HashSet<ClientMealType>();
            ClientClientTypes = new HashSet<ClientClientType>();
            ClientCuisines = new HashSet<ClientCuisine>();
            News = new HashSet<News>();
            Waiters = new HashSet<Waiter>();
            Events = new HashSet<Event>();
            ClientImages = new HashSet<ClientImage>();
            ClientRatings = new HashSet<ClientRating>();
            ClientFavourites = new HashSet<ClientFavourite>();
            ClientSpecialDiets = new HashSet<ClientSpecialDiet>();
            ClientDishes = new HashSet<ClientDish>();
            ClientGoodFors = new HashSet<ClientGoodFor>();
            ClientFeatures = new HashSet<ClientFeature>();
            Reservations = new HashSet<Reservation>();
        }
    }
}



//    [Required]
//    [MaxLength(100)]
//    //[DataType(DataType.EmailAddress)]
//    [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")]
//    public string Email { get; set; }
