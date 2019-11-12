using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class ClientCuisine
    {
       // [Key]
       // public int Id { get; set; }
                                          
         [Key]
        [Column(Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int ClientId { get; set; }

        [Key]
        [Column(Order = 2)]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int CuisineId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("CuisineId")]
        public virtual Cuisine Cuisine { get; set; }
    }
}
