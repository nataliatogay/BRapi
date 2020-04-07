using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Users
{
    public class NewUserRequest
    {
        [Required]
        public string FirstName { get; set; }        
        
        [Required]
        public string LastName { get; set; }
        
        public bool Gender { get; set; }

        [Required]
        public string BirthDate { get; set; }
        
        //public string Email { get; set; }
    }
}
