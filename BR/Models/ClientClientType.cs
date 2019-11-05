using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BR.Models
{
    public class ClientClientType
    {
        [Key]
        [Column(Order = 1)]
        public int ClientId { get; set; }

        [Key]
        [Column(Order = 2)]
        public int ClientTypeId { get; set; }

        [ForeignKey("ClientId")]
        public virtual Client Client { get; set; }

        [ForeignKey("ClientTypeId")]
        public virtual ClientType ClientType { get; set; }
    }
}
