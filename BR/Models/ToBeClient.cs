using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class ToBeClient
    {
        public int Id { get; set; }
        public DateTime RegisteredDate { get; set; }

        [Required]
        public string JsonInfo { get; set; }

        public int? ClientId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }    
    }
}
