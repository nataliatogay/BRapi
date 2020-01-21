using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IBlobService
    {
        Task<string> UploadImage(string imageString);
        Task<bool> DeleteImage(string imagePath);
    }
}
