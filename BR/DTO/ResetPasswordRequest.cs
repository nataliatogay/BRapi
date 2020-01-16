using System.ComponentModel.DataAnnotations;

namespace BR.DTO
{
    public class ResetPasswordRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Code { get; set; }
    }
}
