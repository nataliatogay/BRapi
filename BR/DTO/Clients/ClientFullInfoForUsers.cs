using BR.DTO.Events;
using BR.DTO.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientFullInfoForUsers
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public double Lat { get; set; }
        
        public double Long { get; set; }

        public string MainImage { get; set; }

        public int OpenTime { get; set; }
        
        public int CloseTime { get; set; }

        public string Description { get; set; }

        public ICollection<ParameterInfoForUsers> Cuisines { get; set; }

        public ICollection<ParameterInfoForUsers> MealTypes { get; set; }

        public ICollection<ParameterInfoForUsers> ClientTypes { get; set; }

        public ICollection<ParameterInfoForUsers> Features { get; set; }

        public ICollection<string> SocialLinks { get; set; }

        public ICollection<string> Photos { get; set; }

        public ICollection<ClientPhoneInfo> Phones { get; set; }

    }
}
