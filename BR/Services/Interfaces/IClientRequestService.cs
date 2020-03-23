using BR.DTO;
using BR.DTO.Requests;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IClientRequestService
    {
        Task<IEnumerable<RequestInfoResponse>> GetAllClientRequests();
        Task<IEnumerable<RequestInfoResponse>> GetNewClientRequests();
        Task<RequestInfoResponse> GetClientRequest(int id);
        Task<ServerResponse> AddNewClientRequest(NewClientRequestRequest newClientRequest);
        Task<int> NewClientRequestCount();
        

    }
}
