using System.ComponentModel.DataAnnotations;

namespace BR.DTO.Events
{
    public class UpdateEventRequest
    {
        public int EventId { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        public string Date { get; set; }
        public int EventTypeId { get; set; }
    }
}
