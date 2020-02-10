using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class UserUserPhone
    {
        [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int UserPhoneId { get; set; }

        [ForeignKey("UserId")]
        public virtual User User{ get; set; }

        [ForeignKey("UserPhoneId")]
        public virtual UserPhone UserPhone{ get; set; }
    }
}
