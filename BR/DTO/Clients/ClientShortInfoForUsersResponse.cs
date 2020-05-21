 using BR.DTO.Events;
using BR.DTO.Parameters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class ClientShortInfoForUsersResponse
    {
        public int TotalCount { get; set; }

        public ICollection<ClientShortInfoForUsers> Clients { get; set; }
    }


    public class ClientShortInfoForUsers
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string MainImage { get; set; }

        public string LogoPath { get; set; }

        public List<ParameterInfoForUsers> ClientTypes { get; set; }

        public List<ParameterInfoForUsers> MealTypes { get; set; }

        public double Lat { get; set; }

        public double Long { get; set; }

        public int PriceCategory { get; set; }

        public int TableTotalCount { get; set; }

        public int TableAvailableCount { get; set; } // change

    }

   
}
