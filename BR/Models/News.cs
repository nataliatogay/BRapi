using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class News
    {
        public int Id { get; set; }

        public int ClientId { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public string Description { get; set; }

        public virtual Client Client { get; set; }
    }
}
