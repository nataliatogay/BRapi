using System.ComponentModel.DataAnnotations;

namespace BR.DTO
{
    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string BirthDate { get; set; }
    }
}
