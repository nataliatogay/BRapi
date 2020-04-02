using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Privileges
{
    public class AssignPrivilegeRequest
    {
        public int UserId { get; set; }
        public int PrivilegeId { get; set; }
    }
}
