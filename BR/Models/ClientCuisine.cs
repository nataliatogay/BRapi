using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class ClientCuisine
    {
        [Key]
        [Column(Order = 1)]
        public int ClientId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int CuisineId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("CuisineId")]
        public virtual Cuisine Cuisine { get; set; }
    }
}
