using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Redis
{
    public class BarCurrentStateCacheData
    {
        public int BarId { get; set; }
        public DateTime DateTime { get; set; }
        public int ConfirmedCount { get; set; }
        public int NotConfirmedCount { get; set; }
    }
}
