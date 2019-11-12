using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class ClientMail
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public int AdminId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsRead { get; set; }
        public DateTime TimeSend { get; set; }

        public virtual Client Client { get; set; }
        public virtual Admin Admin { get; set; }
    }
}
