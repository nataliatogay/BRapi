﻿using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class ClientPhone
    {
       
        public int Id { get; set; }

        [Required]
        public string Number { get; set; }

        public int ClientId { get; set; }

                
        public bool IsWhatsApp { get; set; }



        public virtual Client Client { get; set; }
    }
}
