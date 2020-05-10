using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Schema
{
    public class ClientHallsInfo
    {
        public int ClientId { get; set; }
        public ICollection<FloorInfo> Floors { get; set; }

    }
    
}
