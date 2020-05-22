using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Schema
{
    public class TableObject
    {
        public int? Id { get; set; }
        [Required]
        public string Key { get; set; }
        [Required]
        public string Src { get; set; }
        public int Angle { get; set; }
        [Required]
        public string Pos { get; set; }
        public string Group { get; set; }
        public int MaxGuests { get; set; }
        public int MinGuests { get; set; }
        public string Code { get; set; }
        public string Fig { get; set; }
        public string Size { get; set; }
    }
}
