using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class PhoneCode
    {
        public int Id { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public int Code { get; set; }

        public virtual ICollection<ClientPhone> ClientPhones { get; set; }
        public virtual ICollection<User> Users { get; set; }

        public PhoneCode()
        {
            ClientPhones = new HashSet<ClientPhone>();
            Users = new HashSet<User>();
        }
    }
}
