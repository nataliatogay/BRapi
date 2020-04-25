using System.ComponentModel.DataAnnotations;

namespace BR.DTO.Waiters
{
    public class NewWaiterByClientRequest
    {
        [Required]
        public string FirstName { get; set; }

        [Required]
        public string LastName { get; set; }

        [Required]
        public string PhoneNumber { get; set; }

        public bool Gender { get; set; }
    }
}
