using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class ClientPhone
    {
        [Key]
        public string PhoneNumber { get; set; }

        public int ClientId { get; set; }

        public int PhoneCodeId { get; set; }

        public virtual Client Client { get; set; }
        public virtual PhoneCode PhoneCode { get; set; }
    }
}
