using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Waiters
{
    public class WaiterInfo
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string PhoneNumber { get; set; }

        public string Login { get; set; }

        public bool Gender { get; set; }

        public bool IsHeadWaiter { get; set; }
    }
}
