using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Account
{
    public class ChangeClientPasswordByOwnerRequest
    {
        public int ClientId { get; set; }

        [Required]
        public string NewPassword { get; set; }
    }
}
