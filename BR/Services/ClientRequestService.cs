using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class ClientRequestService :IClientRequestService
    {
        private readonly IAsyncRepository _repository;
        private readonly IBlobService _blobService;
        public ClientRequestService(IAsyncRepository repository,
            IBlobService blobService)
        {
            _repository = repository;
            _blobService = blobService;
        }
        public async Task AddNewClientRequest(ClientRequest clientRequest)
        {

            await _repository.AddClientRequest(clientRequest);
        }

        public async Task AddNewClientRequest(NewRequestRequest newClientRequest)
        {
            var imagePath = await _blobService.UploadImage(newClientRequest.MainImage);
            newClientRequest.MainImage = imagePath;
            ClientRequest clientRequest = new ClientRequest()
            {
                RegisteredDate = DateTime.Now,
                JsonInfo = JsonConvert.SerializeObject(newClientRequest)
            };
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
