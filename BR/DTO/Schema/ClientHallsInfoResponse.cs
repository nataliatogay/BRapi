using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Schema
{
    public class ClientHallsInfoResponse
    {
        public int ClientId { get; set; }
        public ICollection<FloorInfo> Floors { get; set; }

    }

    public class FloorInfo
    {
        public int Number { get; set; }
        public ICollection<HallInfo> Halls { get; set; }
    }

    public class HallInfo
    {
        public string Title { get; set; }
        public string JsonInfo { get; set; }
    }
}
