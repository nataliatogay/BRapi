using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class NewEventRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Date { get; set; }
        public string Image { get; set; }
    }
}
