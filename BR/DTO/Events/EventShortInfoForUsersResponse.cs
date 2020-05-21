using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Events
{
    public class EventShortInfoForUsersResponse
    {
        public int TotalCount { get; set; }

        public ICollection<EventShortInfoForUsers> Events { get; set; }
    }

    public class EventShortInfoForUsers
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ImagePath { get; set; }

        public DateTime DateTime { get; set; }

        public int EntranceFee { get; set; }
    }
}
