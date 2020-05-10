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
    public interface IOwnerRequestService
    {
        Task<ServerResponse<IEnumerable<OwnerRequestInfo>>> GetAllOwnerRequests();

        Task<ServerResponse<ICollection<OwnerRequestInfo>>> GetAllOwnerRequests(int take, int skip);

        
        Task<ServerResponse<OwnerRequestInfo>> GetOwnerRequest(int id);
        
        Task<ServerResponse> AddNewOwnerRequest(NewOwnerRequestRequest newClientRequest);
        
        //Task<ServerResponse<int>> UndoneClientRequestCount();

        Task<ServerResponse<int>> OwnerRequestCount();

        Task<ServerResponse> DeclineOwnerRequest(int requestId);


    }
}
