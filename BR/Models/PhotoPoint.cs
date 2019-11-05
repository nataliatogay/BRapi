using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class PhotoPoint
    {
        public int Id { get; set; }

        [Required]
        public int X { get; set; }

        [Required]
        public int Y { get; set; }

        [Required]
        public string ImagePath { get; set; }

        [Required]
        public int HallId { get; set; }

        public virtual Hall Hall { get; set; }
    }
}
