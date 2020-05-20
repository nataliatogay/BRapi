using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Events
{
    public class EventInfoShort
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public int MarkCount { get; set; }

        public bool IsPosted { get; set; }

        public bool IsCancelled { get; set; }

        public int EntranceFee { get; set; }

    }
}
