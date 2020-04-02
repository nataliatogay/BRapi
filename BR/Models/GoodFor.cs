using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class GoodFor
    {
        [Key]
        public int Id { get; set; }
        
        [Required]
        public string Title { get; set; }

        public virtual ICollection<ClientGoodFor> ClientGoodFors{ get; set; }

        public GoodFor()
        {
            ClientGoodFors= new HashSet<ClientGoodFor>();
        }
    }
}
