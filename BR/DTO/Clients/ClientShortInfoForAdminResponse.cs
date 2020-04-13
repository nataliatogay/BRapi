using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientShortInfoForAdminResponse
    {
        public int Id { get; set; }

        public string ClientName { get; set; }
        
        public string OrganizationName { get; set; }
        
        public string MainImage { get; set; }
        
        public string Email { get; set; }
        
        public DateTime RegistrationDate { get; set; }
        
        public bool IsBlocked { get; set; }
    }
}
