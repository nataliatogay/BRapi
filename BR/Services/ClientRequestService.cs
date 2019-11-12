using BR.EF;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class ClientRequestService :IClientRequestService
    {
        private readonly IAsyncRepository _repository;
        public ClientRequestService(IAsyncRepository repository)
        {
            _repository = repository;
        }
        public async Task AddNewClientRequest(ClientRequest clientRequest)
        {
            await _repository.AddClientRequest(clientRequest);
        }

        public async Task<IEnumerable<ClientRequest>> GetAllClientRequests()
        {
            return await _repository.GetClientRequests();
        }

        public async Task<ClientRequest> GetClientRequest(int id)
        {
            return await _repository.GetClientRequest(id);
        }

        public async Task<IEnumerable<PaymentType>> GetAllPaymentTypes()
        {
            return await _repository.GetAllPaymentTypes();
        }
        public async Task<IEnumerable<Cuisine>> GetAllCuisines()
        {
            return await _repository.GetAllCuisines();
        }
        public async Task<IEnumerable<ClientType>> GetAllClientTypes()
        {
            return await _repository.GetAllClientTypes();
        }

        public async Task<int> ClientRequestCount()
        {
            var res = await _repository.GetClientRequests();
            if (res is null)
            {
                return 0;
            }
            return res.Count();
        }
    }
}
