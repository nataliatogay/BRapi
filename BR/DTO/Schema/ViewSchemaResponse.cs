using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Schema
{
    public class ViewSchemaResponse
    {
        public int HallTitle { get; set; }
        public int FloorNumber { get; set; }
        public IEnumerable<TableObject> TableArray { get; set; }
        public IEnumerable<HallObject> NodeArray { get; set; }
        public IEnumerable<GroupObject> GroupArray { get; set; }
    }
}
