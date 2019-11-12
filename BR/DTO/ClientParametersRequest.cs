using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class ClientParametersRequest
    {
        public Dictionary<int, string> Cuisines { get; set; }
        public Dictionary<int, string> PaymentTypes { get; set; }
        public Dictionary<int, string> ClientTypes { get; set; }

        //public IEnumerable<Cuisine> Cuisines { get; set; }
        //public IEnumerable<PaymentType> PaymentTypes { get; set; }
        //public IEnumerable<ClientType> ClientTypes { get; set; }

    }
}
