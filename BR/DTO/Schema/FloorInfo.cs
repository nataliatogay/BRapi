using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Schema
{
    public class FloorInfo
    {
        public int Number { get; set; }
        public ICollection<HallInfo> Halls { get; set; }
    }
}
