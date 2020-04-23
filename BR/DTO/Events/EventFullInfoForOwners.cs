using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Events
{
    public class EventFullInfo
    {
        public int Id { get; set; }
        
        public string Title { get; set; }
        
        public string ImagePath { get; set; }
        
        public string Description { get; set; }

        public DateTime Date { get; set; }

        public int Duration { get; set; }

        public int EntranceFee { get; set; }

        public int MarkCount { get; set; }
    }
}
