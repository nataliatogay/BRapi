using System.ComponentModel.DataAnnotations;

namespace BR.DTO.Users
{
    public class UpdateUserRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        public bool Gender { get; set; }

        [Required]
        public string BirthDate { get; set; }
    }
}
