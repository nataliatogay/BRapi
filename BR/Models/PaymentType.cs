using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace BR.Models
{
    public class PaymentType
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public virtual ICollection<ClientPaymentType> ClientPaymentTypes { get; set; }

        public PaymentType()
        {
            ClientPaymentTypes = new HashSet<ClientPaymentType>();
        }
    }
}
