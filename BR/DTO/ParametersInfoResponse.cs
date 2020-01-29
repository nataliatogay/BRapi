using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO
{
    public class ParametersInfoResponse
    {
        public ICollection<ParameterInfo> Cuisines { get; set; }
        public ICollection<ParameterInfo> PaymentTypes { get; set; }
        public ICollection<ParameterInfo> ClientTypes { get; set; }
        public ICollection<ParameterInfo> MealTypes { get; set; }
    }

    public class ParameterInfo
    {
        public int Id { get; set; }
        public string Title { get; set; }
    }
}
