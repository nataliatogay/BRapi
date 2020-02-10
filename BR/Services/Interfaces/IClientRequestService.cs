using BR.DTO;
using BR.DTO.Requests;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IClientRequestService
    {
        Task<IEnumerable<RequestInfoResponse>> GetAllClientRequests();
        Task<RequestInfoResponse> GetClientRequest(int id);
        Task AddNewClientRequest(ClientRequest clientRequest);
        Task AddNewClientRequest(NewRequestRequest newClientRequest);
        Task<int> ClientRequestCount();
        

    }
}
