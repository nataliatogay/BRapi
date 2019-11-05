using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class ClientType
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<ClientClientType> ClientClientTypes { get; set; }

        public ClientType()
        {
            ClientClientTypes = new HashSet<ClientClientType>();
        }
    }
}
