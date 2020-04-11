﻿using BR.DTO;
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
        public ClientRequestService(IAsyncRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServerResponse> AddNewClientRequest(NewClientRequestRequest newClientRequest)
        {

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
                var request = await _repository.AddClientRequest(clientRequest);

                var notificationType = await _repository.GetNotificationType("request");
                if (notificationType is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }


                await _repository.AddAdminNotification(new AdminNotification()
                {
                    DateTime = request.RegisteredDate,
                    NotificationTypeId = notificationType.Id,
                    RequestId = request.Id,
                    Title = request.OrganizationName
                });

                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }

        public async Task<ServerResponse<IEnumerable<RequestInfoResponse>>> GetAllClientRequests()
        {
            IEnumerable<ClientRequest> requests;
            try
            {

                requests = await _repository.GetClientRequests();
                var requestsInfo = new List<RequestInfoResponse>();
                if (requests is null)
                {
                    requestsInfo = null;
                }
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
                        RegisteredDate = item.RegisteredDate,
                        OwnerId = item.OwnerId
                    });
                }
                return new ServerResponse<IEnumerable<RequestInfoResponse>>(StatusCode.Ok, requestsInfo);
            }
            catch
            {
                return new ServerResponse<IEnumerable<RequestInfoResponse>>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<ICollection<RequestInfoResponse>>> GetAllClientRequests(int take, int skip)
        {
            ICollection<ClientRequest> requests;

            try
            {
                requests = await _repository.GetClientRequests(take, skip);
                var requestsInfo = new List<RequestInfoResponse>();
                if (requests is null)
                {
                    requestsInfo = null;
                }
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
                        RegisteredDate = item.RegisteredDate,
                        OwnerId = item.OwnerId
                    });
                }
                return new ServerResponse<ICollection<RequestInfoResponse>>(StatusCode.Ok, requestsInfo);
            }
            catch
            {
                return new ServerResponse<ICollection<RequestInfoResponse>>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse<RequestInfoResponse>> GetClientRequest(int id)
        {
            ClientRequest request;

            try
            {
                request = await _repository.GetClientRequest(id);
                if (request is null)
                {
                    return new ServerResponse<RequestInfoResponse>(StatusCode.NotFound, null);
                }
                return new ServerResponse<RequestInfoResponse>(StatusCode.Ok,
                new RequestInfoResponse()
                {
                    Id = request.Id,
                    OwnerName = request.OwnerName,
                    OrganizationName = request.OrganizationName,
                    OwnerPhoneNumber = request.OwnerPhoneNumber,
                    Email = request.Email,
                    Comments = request.Comments,
                    RegisteredDate = request.RegisteredDate,
                    OwnerId = request.OwnerId
                });
            }
            catch
            {
                return new ServerResponse<RequestInfoResponse>(StatusCode.DbConnectionError, null);
            }
        }

        //public async Task<ServerResponse<int>> UndoneClientRequestCount()
        //{
        //    IEnumerable<ClientRequest> requests;

        //    try
        //    {
        //        requests = await _repository.GetUndoneClientRequests();
        //        int count = 0;
        //        if (requests != null)
        //        {
        //            count = requests.Count();
        //        }
        //        return new ServerResponse<int>(StatusCode.Ok, count);
        //    }
        //    catch
        //    {
        //        return new ServerResponse<int>(StatusCode.DbConnectionError, 0);
        //    }
        //}


        public async Task<ServerResponse<int>> ClientRequestCount()
        {
            IEnumerable<ClientRequest> requests;

            try
            {
                requests = await _repository.GetClientRequests();
                int count = 0;
                if (requests != null)
                {
                    count = requests.Count();
                }
                return new ServerResponse<int>(StatusCode.Ok, count);
            }
            catch
            {
                return new ServerResponse<int>(StatusCode.DbConnectionError, 0);
            }
        }


        public async Task<ServerResponse> DeclineRequest(int requestId)
        {
            ClientRequest request;
            try
            {
                request = await _repository.GetClientRequest(requestId);
                if (request is null || request.AdminNotification is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
                var notification = request.AdminNotification;
                notification.Done = false;
                await _repository.UpdateAdminNotification(notification);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }
    }
}
