using BR.DTO;
using BR.DTO.Requests;
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
    public class ClientRequestService : IClientRequestService
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
            string imagePath;
            if (!String.IsNullOrEmpty(newClientRequest.MainImage))
            {

                imagePath = await _blobService.UploadImage(newClientRequest.MainImage);
            }
            else
            {
                newClientRequest.MainImage = "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg";

            }
            ClientRequest clientRequest = new ClientRequest()
            {
                RegisteredDate = DateTime.Now,
                JsonInfo = JsonConvert.SerializeObject(newClientRequest)
            };
            await _repository.AddClientRequest(clientRequest);
        }

        public async Task<IEnumerable<RequestInfoResponse>> GetAllClientRequests()
        {
            var requests = await _repository.GetClientRequests();
            var requestsInfo = new List<RequestInfoResponse>();
            foreach (var item in requests)
            {
                requestsInfo.Add(new RequestInfoResponse()
                {
                    Id = item.Id,
                    RegisteredDate = item.RegisteredDate,
                    JsonInfo = item.JsonInfo
                });
            }
            {

            }
            return requestsInfo;
        }

        public async Task<RequestInfoResponse> GetClientRequest(int id)
        {
            var req = await _repository.GetClientRequest(id);
            return new RequestInfoResponse()
            {
                Id = req.Id,
                RegisteredDate = req.RegisteredDate,
                JsonInfo = req.JsonInfo
            };
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
