using System.ComponentModel.DataAnnotations;

namespace BR.DTO
{
    public class LogInRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        //[MinLength(3)]
        public string Password { get; set; }

        [Required]
        public string NotificationTag { get; set; }
    }
}
