using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Requests
{
    public class RequestInfoResponse
    {
        public int Id { get; set; }
        public DateTime RegisteredDate { get; set; }
        public string JsonInfo { get; set; }
    }
}
