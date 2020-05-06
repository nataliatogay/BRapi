using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Test2
    {
        [Key]
        public int Test1Id { get; set; }
        
        [ForeignKey("Test1Id")]
        public virtual Test1 Test1 { get; set; }

    }
}






