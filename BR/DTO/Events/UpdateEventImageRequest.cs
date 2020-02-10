using System.ComponentModel.DataAnnotations;

namespace BR.DTO.Events
{
    public class UpdateEventImageRequest
    {
        public int EventId { get; set; }
        [Required]
        public string ImageString { get; set; }
    }
}
