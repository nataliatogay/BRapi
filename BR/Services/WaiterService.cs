using BR.DTO;
using BR.DTO.Waiters;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
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

        public async Task<IEnumerable<Waiter>> GetAllWaiters(string clientIdentityId)
        {
            var client = _repository.GetClient(clientIdentityId);
            if(client is null)
            {
                return null;
            }
            return await _repository.GetWaitersByClientId(client.Id);
        }
        public async Task<Waiter> GetWaiter(int id)
        {
            return await _repository.GetWaiter(id);
        }
        public async Task<Waiter> AddNewWaiter(NewWaiterRequest newWaiterRequest, string identityId, string clientIdentityId)
        {
            var client = await _repository.GetClient(clientIdentityId);
            if(client != null)
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
                if (newWaiterRequest.BirthDate != null)
                {
                  //  waiter.BirthDate = DateTime.ParseExact(newWaiterRequest.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                }
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
