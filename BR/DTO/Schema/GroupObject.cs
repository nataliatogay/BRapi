using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Schema
{
    public class GroupObject
    {
        [Required]
        public string Key { get; set; }
        public int Angle { get; set; }
        [Required]
        public string Pos { get; set; }
        [Required]
        public string Size { get; set; }
        public string Fig { get; set; }
    }
}
