using BR.DTO.Clients;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Events
{
    public class EventFullInfoForUsers
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ImagePath { get; set; }

        public string Description { get; set; }

        public DateTime DateTime { get; set; }

        public int Duration { get; set; }

        public ICollection<ClientPhoneInfo> Phones { get; set; }

        public int EntranceFee { get; set; }

        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public int EventMarkCount { get; set; }

        public ICollection<EventMarkedUser> Users { get; set; }
    }

    public class EventMarkedUser
    {
        public int UserId { get; set; }
        public string UserImagePath { get; set; }
    }
}
