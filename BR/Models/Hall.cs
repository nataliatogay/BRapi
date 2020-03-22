using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class Hall
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public int FloorId { get; set; }

        [Required]
        public string JsonInfo { get; set; }

        public virtual Floor Floor { get; set; }

        public virtual ICollection<Table> Tables { get; set; }
        public virtual ICollection<BarTable> BarTables { get; set; }
        public virtual ICollection<PhotoPoint> PhotoPoints { get; set; }

        public Hall()
        {
            Tables = new HashSet<Table>();
            BarTables = new HashSet<BarTable>();
            PhotoPoints = new HashSet<PhotoPoint>();
        }

    }
}
