using BR.DTO;
using BR.DTO.Privileges;
using BR.DTO.Waiters;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IWaiterService
    {

        Task<ServerResponse<WaiterInfo>> AddNewWaiterByOwner(NewWaiterByOwnerRequest waiterRequest, string waiterIdentityId, string ownerIdentityId);

        Task<ServerResponse<WaiterInfo>> AddNewWaiterByClient(NewWaiterByClientRequest waiterRequest, string waiterIdentityId, string clientIdentityId);

        Task<ServerResponse<ICollection<WaiterInfo>>> GetAllWaitersForOwner(int clientId, string ownerIdentityId);

        Task<ServerResponse<ICollection<WaiterInfo>>> GetAllWaitersForClient(string clientIdentityId);

        Task<ServerResponse<WaiterInfo>> UpdateWaiterByOwner(UpdateWaiterRequest updateRequest, string ownerIdentityId);

        Task<ServerResponse<WaiterInfo>> UpdateWaiterByClient(UpdateWaiterRequest updateRequest, string clientIdentityId);



        //=============================================================================================================


        Task<IEnumerable<Waiter>> GetAllWaiters(string clientIdentityId);
        Task<Waiter> GetWaiter(int id);
        Task<Waiter> AddNewWaiter(NewWaiterByClientRequest newWaiterRequest, string identityId, string clientIdentityId);
        Task<Waiter> UpdateWaiter(Waiter waiter);
        Task<bool> DeleteWaiter(int id);

        Task<ServerResponse> AssignPrivilege(AssignPrivilegeRequest assignmentRequest);

        string GenerateLogin(string lastName);
        string GeneratePassword();
    }
}
