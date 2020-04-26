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

        //public string ImageString { get; set; }

        [Required]
        public string Date { get; set; }

        public int Duration { get; set; }

        public int EntranceFee { get; set; }
    }
}
