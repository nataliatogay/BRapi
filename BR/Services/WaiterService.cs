using BR.DTO;
using BR.DTO.Privileges;
using BR.DTO.Waiters;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR.Services
{
    public class WaiterService : IWaiterService
    {
        private readonly IAsyncRepository _repository;

        public WaiterService(IAsyncRepository repository)
        {
            _repository = repository;
        }


        public async Task<ServerResponse<WaiterInfo>> AddNewWaiterByOwner(NewWaiterByOwnerRequest waiterRequest, string waiterIdentityId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<WaiterInfo>(StatusCode.UserNotFound, null);
                }
                Client client = await _repository.GetClient(waiterRequest.ClientId);
                if (client is null || owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(client))
                {
                    return new ServerResponse<WaiterInfo>(StatusCode.NotAvailable, null);
                }

            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }
            Waiter waiter = new Waiter()
            {
                ClientId = waiterRequest.ClientId,
                FirstName = waiterRequest.FirstName,
                LastName = waiterRequest.LastName,
                Gender = waiterRequest.Gender,
                IdentityId = waiterIdentityId
            };

            try
            {
                waiter = await _repository.AddWaiter(waiter);
                return new ServerResponse<WaiterInfo>(StatusCode.Ok, this.WaiterToWaiterInfo(waiter));
            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<WaiterInfo>> AddNewWaiterByClient(NewWaiterByClientRequest waiterRequest, string waiterIdentityId, string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<WaiterInfo>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }
            Waiter waiter = new Waiter()
            {
                ClientId = client.Id,
                FirstName = waiterRequest.FirstName,
                LastName = waiterRequest.LastName,
                Gender = waiterRequest.Gender,
                IdentityId = waiterIdentityId
            };

            try
            {
                waiter = await _repository.AddWaiter(waiter);
                return new ServerResponse<WaiterInfo>(StatusCode.Ok, this.WaiterToWaiterInfo(waiter));
            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }
        }



        public async Task<ServerResponse<ICollection<WaiterInfo>>> GetAllWaitersForOwner(int clientId, string ownerIdentityId)
        {
            Owner owner;
            Client client;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<ICollection<WaiterInfo>>(StatusCode.UserNotFound, null);
                }
                client = await _repository.GetClient(clientId);
                if (client is null || owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(client))
                {
                    return new ServerResponse<ICollection<WaiterInfo>>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<WaiterInfo>>(StatusCode.DbConnectionError, null);
            }

            var waiters = client.Waiters;
            var result = new List<WaiterInfo>();
            foreach (var item in waiters)
            {
                result.Add(this.WaiterToWaiterInfo(item));
            }

            return new ServerResponse<ICollection<WaiterInfo>>(StatusCode.Ok, result);
        }


        public async Task<ServerResponse<ICollection<WaiterInfo>>> GetAllWaitersForClient(string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<WaiterInfo>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<WaiterInfo>>(StatusCode.DbConnectionError, null);
            }
            var waiters = client.Waiters;
            var result = new List<WaiterInfo>();
            foreach (var item in waiters)
            {
                result.Add(this.WaiterToWaiterInfo(item));
            }

            return new ServerResponse<ICollection<WaiterInfo>>(StatusCode.Ok, result);
        }



        public async Task<ServerResponse<WaiterInfo>> UpdateWaiterByOwner(UpdateWaiterRequest updateRequest, string ownerIdentityId)
        {
            Owner owner;
            Waiter waiter;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<WaiterInfo>(StatusCode.UserNotFound, null);
                }
                waiter = await _repository.GetWaiter(updateRequest.WaiterId);
                if (waiter is null || owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(waiter.Client))
                {
                    return new ServerResponse<WaiterInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }

            waiter.FirstName = updateRequest.FirstName;
            waiter.LastName = updateRequest.LastName;
            waiter.Gender = updateRequest.Gender;
            try
            {
                waiter = await _repository.UpdateWaiter(waiter);
                return new ServerResponse<WaiterInfo>(StatusCode.Ok, this.WaiterToWaiterInfo(waiter));
            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<WaiterInfo>> UpdateWaiterByClient(UpdateWaiterRequest updateRequest, string clientIdentityId)
        {
            Client client;
            Waiter waiter;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<WaiterInfo>(StatusCode.UserNotFound, null);
                }
                waiter = await _repository.GetWaiter(updateRequest.WaiterId);
                if (waiter is null || !client.Waiters.Contains(waiter))
                {
                    return new ServerResponse<WaiterInfo>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }

            waiter.FirstName = updateRequest.FirstName;
            waiter.LastName = updateRequest.LastName;
            waiter.Gender = updateRequest.Gender;
            try
            {
                waiter = await _repository.UpdateWaiter(waiter);
                return new ServerResponse<WaiterInfo>(StatusCode.Ok, this.WaiterToWaiterInfo(waiter));
            }
            catch
            {
                return new ServerResponse<WaiterInfo>(StatusCode.DbConnectionError, null);
            }
        }



        private WaiterInfo WaiterToWaiterInfo(Waiter waiter)
        {
            if (waiter is null)
            {
                return null;
            }
            return new WaiterInfo()
            {
                Id = waiter.Id,
                FirstName = waiter.FirstName,
                LastName = waiter.LastName,
                Gender = waiter.Gender,
                Login = waiter.Identity.UserName,
                PhoneNumber = waiter.Identity.PhoneNumber,

                // change
                IsHeadWaiter = false
            };
        }



        //===========================================================================================================================

        public async Task<IEnumerable<Waiter>> GetAllWaiters(string clientIdentityId)
        {
            var client = await _repository.GetClient(clientIdentityId);
            if (client is null)
            {
                return null;
            }
            return client.Waiters;
        }
        public async Task<Waiter> GetWaiter(int id)
        {
            return await _repository.GetWaiter(id);
        }
        public async Task<Waiter> AddNewWaiter(NewWaiterByClientRequest newWaiterRequest, string identityId, string clientIdentityId)
        {
            var client = await _repository.GetClient(clientIdentityId);
            if (client != null)
            {
                Waiter waiter = new Waiter()
                {
                    FirstName = newWaiterRequest.FirstName,
                    LastName = newWaiterRequest.LastName,
                    //   Gender = newWaiterRequest.Gender,
                    //   BirthDate = null,
                    ClientId = client.Id,
                    IdentityId = identityId
                };
                return await _repository.AddWaiter(waiter);

            }
            return null;
        }
        public async Task<Waiter> UpdateWaiter(Waiter waiter)
        {
            var waiterToUpdate = await _repository.GetWaiter(waiter.Id);
            if (waiterToUpdate is null)
            {
                return null;
            }
            waiterToUpdate.FirstName = waiter.FirstName;
            waiterToUpdate.LastName = waiter.LastName;
            await _repository.UpdateWaiter(waiterToUpdate);
            return waiterToUpdate;
        }
        public async Task<bool> DeleteWaiter(int id)
        {
            var waiter = await _repository.GetWaiter(id);
            if (waiter is null)
            {
                return false;
            }
            return await _repository.DeleteWaiter(waiter);

        }


        public async Task<ServerResponse> AssignPrivilege(AssignPrivilegeRequest assignmentRequest)
        {
            Waiter waiter;
            try
            {
                waiter = await _repository.GetWaiter(assignmentRequest.UserId);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (waiter is null)
            {
                return new ServerResponse(StatusCode.UserNotFound);
            }
            try
            {
                var res = _repository.AddUserPrivilage(new UserPrivileges()
                {
                    IdentityId = waiter.IdentityId,
                    PrivilegeId = assignmentRequest.PrivilegeId
                });
                return new ServerResponse(StatusCode.Ok);
            }
            catch (DbUpdateException)
            {
                return new ServerResponse(StatusCode.Duplicate);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
        }

        public string GenerateLogin(string lastName)
        {
            StringBuilder login = new StringBuilder();
            _ = (lastName.Length > 4) ? login.Append(lastName.Substring(0, 4)) : login.Append(lastName);
            login.Append("_");
            string letters = "abcdefghijklmnopqrstuvwxyz";
            Random random = new Random();
            login.Append(letters.ElementAt(random.Next(0, letters.Length)));
            login.Append(letters.ElementAt(random.Next(0, letters.Length)));
            login.Append(random.Next(0, 10).ToString());
            login.Append(random.Next(0, 10).ToString());
            return login.ToString();

        }
        public string GeneratePassword()
        {
            Random random = new Random();
            string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 8; ++i)
            {
                password.Append(letters.ElementAt(random.Next(0, letters.Length)));
            }
            return password.ToString();
        }

    }
}
