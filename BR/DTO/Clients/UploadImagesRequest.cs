using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class UploadImagesRequest
    {
        public int ClientId { get; set; }
        public ICollection<string> ImageStrings { get; set; }
    }
}
