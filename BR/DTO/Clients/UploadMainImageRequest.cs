using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BR.DTO.Clients
{
    public class UploadMainImageRequest
    {
        public int ClientId { get; set; }

        [Required]
        public string ImageString { get; set; }
    }
}
