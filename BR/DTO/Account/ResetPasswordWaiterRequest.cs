using System.ComponentModel.DataAnnotations;

namespace BR.DTO.Account
{
    public class ResetPasswordWaiterRequest
    {
        [Required]
        public string Login { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Code { get; set; }
    }
}