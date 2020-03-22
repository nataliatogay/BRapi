 using BR.DTO.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{

    public class ClientShortInfoForUsersResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string MainImage { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

   
}
