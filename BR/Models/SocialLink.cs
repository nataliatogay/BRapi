using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class SocialLink
    {
        public int Id { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        public int ClientId { get; set; }

        public virtual Client Client { get; set; }
    }
}
