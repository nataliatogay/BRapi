using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class Admin
    {
        public int Id { get; set; }

       // [Required]
       // [MaxLength(100)]
        //[DataType(DataType.EmailAddress)]
       // [RegularExpression(@"(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|""(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21\x23-\x5b\x5d-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])*"")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\x01-\x08\x0b\x0c\x0e-\x1f\x21-\x5a\x53-\x7f]|\\[\x01-\x09\x0b\x0c\x0e-\x7f])+)\])")]
       // public string Email { get; set; }

      //  [Required]
      //  public string Password { get; set; }

        public string IdentityId { get; set; }

        [ForeignKey("IdentityId")]
        public virtual IdentityUser Identity { get; set; }

        public virtual ICollection<AccountToken> AccountTokens { get; set; }
        //public virtual ICollection<ClientMail> ClientMails { get; set; }
        //public virtual ICollection<UserMail> UserMails { get; set; }


        public Admin()
        {
            AccountTokens = new HashSet<AccountToken>();
           // ClientMails = new HashSet<ClientMail>();
           // UserMails = new HashSet<UserMail>();
        }
    }
}
