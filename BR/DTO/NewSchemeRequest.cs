using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class NewSchemaRequest
    {
        public int FloorNumber { get; set; }
        [Required]
        public string HallTitle { get; set; }
        public IEnumerable<TableObject> TableArray { get; set; }
        public IEnumerable<HallObject> NodeArray { get; set; }
        public IEnumerable<GroupObject> GroupArray { get; set; }
    }

    public class UpdateSchemaRequest
    {
        public int HallId { get; set; }
        [Required]
        public string HallTitle { get; set; }
        public IEnumerable<TableObject> TableArray { get; set; }
        public IEnumerable<HallObject> NodeArray { get; set; }
        public IEnumerable<GroupObject> GroupArray { get; set; }
    }


    public class ViewSchemaResponse
    {
        public int HallTitle { get; set; }
        public int FloorNumber { get; set; }
        public IEnumerable<TableObject> TableArray { get; set; }
        public IEnumerable<HallObject> NodeArray { get; set; }
        public IEnumerable<GroupObject> GroupArray { get; set; }
    }


    public class GroupObject
    {
        [Required]
        public string Key { get; set; }
        public int Angle { get; set; }
        [Required]
        public string Pos { get; set; }
        [Required]
        public string Size { get; set; }
    }

    public class HallObject
    {
        [Required]
        public string Key { get; set; }
        [Required]
        public string Src { get; set; }
        public int Angle { get; set; }
        [Required]
        public string Pos { get; set; }
        public string Group { get; set; }
    }

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
        public int Number { get; set; }
    }
}
