using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class UserPhone
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public virtual ICollection<UserUserPhone> UserUserPhones{ get; set; }
        public UserPhone()
        {
            UserUserPhones = new HashSet<UserUserPhone>();
        }
    }
}
