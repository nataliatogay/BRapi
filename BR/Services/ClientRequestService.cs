using BR.DTO;
using BR.DTO.Requests;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
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
        
        public async Task<ServerResponse> AddNewClientRequest(NewClientRequestRequest newClientRequest)
        {
            //  перенести в добавление клиента

            //string imagePath;

            //if (!String.IsNullOrEmpty(newClientRequest.MainImage))
            //{

            //    imagePath = await _blobService.UploadImage(newClientRequest.MainImage);
            //}
            //else
            //{
            //    newClientRequest.MainImage = "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg";

            //}

            ClientRequest clientRequest = new ClientRequest()
            {
                OwnerName = newClientRequest.OwnerName,
                OrganizationName = newClientRequest.OrganizationName,
                OwnerPhoneNumber = newClientRequest.OwnerPhoneNumber,
                Email = newClientRequest.Email,
                Comments = newClientRequest.Comments,
                RegisteredDate = DateTime.Now
            };
            try
            {
                await _repository.AddClientRequest(clientRequest);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
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
                    OwnerName = item.OwnerName,
                    OrganizationName = item.OrganizationName,
                    OwnerPhoneNumber = item.OwnerPhoneNumber,
                    Email = item.Email,
                    Comments = item.Comments,
                    RegisteredDate = item.RegisteredDate
                });
            }
            return requestsInfo;
        }


        public async Task<IEnumerable<RequestInfoResponse>> GetNewClientRequests()
        {
            var requests = await _repository.GetClientRequests();
            var requestsInfo = new List<RequestInfoResponse>();
            foreach (var item in requests)
            {
                if (item.OwnerId is null)
                {
                    requestsInfo.Add(new RequestInfoResponse()
                    {
                        Id = item.Id,
                        OwnerName = item.OwnerName,
                        OrganizationName = item.OrganizationName,
                        OwnerPhoneNumber = item.OwnerPhoneNumber,
                        Email = item.Email,
                        Comments = item.Comments,
                        RegisteredDate = item.RegisteredDate
                    });
                }
            }
            return requestsInfo;
        }

        public async Task<RequestInfoResponse> GetClientRequest(int id)
        {
            var req = await _repository.GetClientRequest(id);
            return new RequestInfoResponse()
            {
                Id = req.Id,
                OwnerName = req.OwnerName,
                OrganizationName = req.OrganizationName,
                OwnerPhoneNumber = req.OwnerPhoneNumber,
                Email = req.Email,
                Comments = req.Comments,
                RegisteredDate = req.RegisteredDate
            };
        }

        public async Task<int> NewClientRequestCount()
        {
            var res = await this.GetNewClientRequests();
            if(res is null)
            {
                return 0;
            }
            return res.Count();
        }
    }
}
