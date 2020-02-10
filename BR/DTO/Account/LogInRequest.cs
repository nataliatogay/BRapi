using System.ComponentModel.DataAnnotations;

namespace BR.DTO.Account
{
    public class LogInRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        public string Password { get; set; }

        [Required]
        public string NotificationTag { get; set; }
    }
}
