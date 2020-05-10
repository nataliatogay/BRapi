using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Parameters
{
    public class ClientParametersInfo
    {
        public ICollection<ParameterInfo> Cuisines { get; set; }

        public ICollection<ParameterInfo> ClientTypes { get; set; }

        public ICollection<ParameterInfo> MealTypes { get; set; }

        public ICollection<ParameterInfo> Features { get; set; }

        public ICollection<ParameterInfo> GoodFors { get; set; }

        public ICollection<ParameterInfo> SpecialDiets { get; set; }

        public ICollection<ParameterInfo> Dishes { get; set; }
    }

    
}
