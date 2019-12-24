using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Event
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImagePath { get; set; }
        public int ClientId { get; set; }
        public virtual Client Client { get; set; }
    }
}
