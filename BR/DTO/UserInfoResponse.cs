using System;

namespace BR.DTO
{
    public class UserInfoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string ImagePath { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
    }
}
