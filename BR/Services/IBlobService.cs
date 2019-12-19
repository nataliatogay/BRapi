using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IBlobService
    {
        Task<string> UploadImage(string imageString);
    }
}
