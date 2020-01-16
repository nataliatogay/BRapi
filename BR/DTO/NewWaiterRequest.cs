using System.ComponentModel.DataAnnotations;

namespace BR.DTO
{
    public class NewWaiterRequest
    {
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        public string BirthDate { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        public bool? Gender { get; set; }
    }
}
