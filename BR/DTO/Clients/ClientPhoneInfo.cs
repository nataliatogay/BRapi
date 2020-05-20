using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientPhoneInfo
    {
        [Required]
        public string Number { get; set; }

        public bool IsWhatsApp { get; set; }

        public bool IsTelegram { get; set; }
    }
}
