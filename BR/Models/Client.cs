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
        [MaxLength(30)]
        public string Name { get; set; }

        [Required]
        [MaxLength(150)]
        public string Address { get; set; }

        [Required]
        public float Lat { get; set; }

        [Required]
        public float Long { get; set; }
        
        [Required]        
        public int OpenTime { get; set; } //  the number of minutes past midnight

        [Required]      
        public int CloseTime { get; set; } //  the number of minutes past midnight

        [Required]
        public bool IsParking { get; set; }

        [Required]
        public bool IsWiFi { get; set; }
        
        public bool? IsLiveMusic { get; set; }

        [Required]
        public bool IsOpenSpace { get; set; }

        [Required]
        public bool IsChildrenZone { get; set; }

        [Required]
        public bool IsBusinessLunch { get; set; }

        [MaxLength(250)]
        public string AdditionalInfo { get; set; }

    //    [Required]
    //    [MaxLength(100)]
    //    //[DataType(DataType.EmailAddress)]
    //    [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")]
    //    public string Email { get; set; }

        //[Required]
       // [MaxLength(20)]
    //    public string Password { get; set; }

        [Required]
        public string MainImagePath { get; set; }

        [Required]
        public int MaxReserveDays { get; set; }
        public int ReserveDurationAvg { get; set; } // in mins

        public bool IsBlocked { get; set; }

        public DateTime RegistrationDate { get; set; }
        public string IdentityId { get; set; }

        [ForeignKey("IdentityId")]
        public virtual IdentityUser Identity { get; set; }
        //public virtual ClientRequest ClientRequest { get; set; }
        public virtual ICollection<SocialLink> SocialLinks { get; set; }
        public virtual ICollection<ClientPhone> ClientPhones { get; set; }
        public virtual ICollection<Floor> Floors { get; set; }
        public virtual ICollection<ClientPaymentType> ClientPaymentTypes { get; set; }
        public virtual ICollection<ClientMealType> ClientMealTypes { get; set; }
        public virtual ICollection<ClientClientType> ClientClientTypes { get; set; }
        public virtual ICollection<ClientCuisine> ClientCuisines { get; set; }
        public virtual ICollection<News> News { get; set; }
       // public virtual ICollection<AccountToken> AccountTokens { get; set; }
        public virtual ICollection<Waiter> Waiters { get; set; }
        public virtual ICollection<Event> Events { get; set; }
        public virtual ICollection<ClientImage> ClientImages { get; set; }
        public virtual ICollection<ClientFavourite> ClientFavourites{ get; set; }



        //public virtual ICollection<ClientMail> ClientMails { get; set; }

        public Client()
        {
            SocialLinks = new HashSet<SocialLink>();
            ClientPhones = new HashSet<ClientPhone>();
            Floors = new HashSet<Floor>();
            ClientPaymentTypes = new HashSet<ClientPaymentType>();
            ClientMealTypes = new HashSet<ClientMealType>();
            ClientClientTypes = new HashSet<ClientClientType>();
            ClientCuisines = new HashSet<ClientCuisine>();
            News = new HashSet<News>();
           // AccountTokens = new HashSet<AccountToken>();
            Waiters = new HashSet<Waiter>();
            Events = new HashSet<Event>();
            ClientImages = new HashSet<ClientImage>();
            // ClientMails = new HashSet<ClientMail>();
        }
    }
}
