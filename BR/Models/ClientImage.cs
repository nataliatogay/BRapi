using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class ClientImage
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
