using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Events
{
    public class NewEventByOwnerRequest
    {
        public int ClientId { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Date { get; set; }

        public int Duration { get; set; }  // in mins

        public int EntranceFee { get; set; }

        public string ImageString { get; set; }
    }
}
