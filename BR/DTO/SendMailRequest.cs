using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class SendMailRequest
    {
        public int RecipentId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
