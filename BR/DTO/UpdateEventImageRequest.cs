using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class UpdateEventImageRequest
    {
        public int EventId { get; set; }
        public string ImageString { get; set; }
    }
}
