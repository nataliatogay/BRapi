using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Parameters
{
    public class ClientParametersForUsers
    {
        public ICollection<ParameterInfoForUsers> Cuisines { get; set; }

        public ICollection<ParameterInfoForUsers> ClientTypes { get; set; }

        public ICollection<ParameterInfoForUsers> MealTypes { get; set; }

        public ICollection<ParameterInfoForUsers> Features { get; set; }

        public ICollection<ParameterInfoForUsers> GoodFors { get; set; }

        public ICollection<ParameterInfoForUsers> SpecialDiets { get; set; }

        public ICollection<ParameterInfoForUsers> Dishes { get; set; }
    }
}
