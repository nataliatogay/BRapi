using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Schema
{
    public class UpdateSchemaRequest
    {
        public int HallId { get; set; }
        [Required]
        public string HallTitle { get; set; }
        public IEnumerable<TableObject> TableArray { get; set; }
        public IEnumerable<HallObject> NodeArray { get; set; }
        public IEnumerable<GroupObject> GroupArray { get; set; }
    }
}