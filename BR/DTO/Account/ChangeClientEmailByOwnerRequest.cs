using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Account
{
    public class ChangeClientEmailByOwnerRequest
    {
        public int ClientId { get; set; }

        public string NewEmail { get; set; }
    }
}
