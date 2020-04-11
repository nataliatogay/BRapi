using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class ClientRequest
    {


        [Key]
        public int Id { get; set; }

        [Required]
        public string OwnerName { get; set; }

        [Required]
        public string OwnerPhoneNumber { get; set; }

        [Required]
        public string OrganizationName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public string Comments { get; set; }

        public DateTime RegisteredDate { get; set; }

        public int? OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public virtual Owner Owner { get; set; }

        public virtual AdminNotification AdminNotification { get; set; }
    }
}

//[Required]
//public string JsonInfo { get; set; }