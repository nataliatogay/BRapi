using BR.DTO.Owners;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IOwnerService
    {
        Task<ServerResponse> AddNewOwner(NewOwnerRequest newOwnerRequest, string identityId);
    }
}
