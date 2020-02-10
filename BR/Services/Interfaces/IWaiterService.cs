using BR.DTO;
using BR.DTO.Waiters;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IWaiterService
    {
        Task<IEnumerable<Waiter>> GetAllWaiters(string clientIdentityId);
        Task<Waiter> GetWaiter(int id);
        Task<Waiter> AddNewWaiter(NewWaiterRequest newWaiterRequest, string identityId, string clientIdentityId);
        Task<Waiter> UpdateWaiter(Waiter waiter);
        Task<bool> DeleteWaiter(int id);

        string GenerateLogin(string lastName);
        string GeneratePassword();
    }
}
