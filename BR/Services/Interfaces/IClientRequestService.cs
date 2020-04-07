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
        Task<ServerResponse<IEnumerable<RequestInfoResponse>>> GetAllClientRequests();

        Task<ServerResponse<ICollection<RequestInfoResponse>>> GetAllClientRequests(int take, int skip);

        
        Task<ServerResponse<RequestInfoResponse>> GetClientRequest(int id);
        
        Task<ServerResponse> AddNewClientRequest(NewClientRequestRequest newClientRequest);
        
        Task<ServerResponse<int>> UndoneClientRequestCount();
        
    }
}
