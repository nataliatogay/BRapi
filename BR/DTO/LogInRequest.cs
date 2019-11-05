using System.ComponentModel.DataAnnotations;

namespace BR.DTO
{
    public class LogInRequest
    {
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [Required(ErrorMessage = "Password is required")]
        [DataType(DataType.Password)]
        [MaxLength(20)]
        //[MinLength(3)]
        public string Password { get; set; }
    }
}
