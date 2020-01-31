using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class BlockUserRequest
    {
        public int UserId { get; set; }
        public bool ToBlock { get; set; }
    }
}
