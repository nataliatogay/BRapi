using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Models
{
    public class Test1
    {
        [Key]
        public int Id { get; set; }

        public virtual Test2 Test2 { get; set; }
    }
}
