using BR.DTO.Clients;
using BR.DTO.Owners;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class OwnerService : IOwnerService
    {
        private readonly IAsyncRepository _repository;

        public OwnerService(IAsyncRepository repository)
        {
            _repository = repository;
        }

        public async Task<ServerResponse> AddNewOwner(NewOwnerRequest newOwnerRequest, string identityId)
        {
            Owner owner = new Owner()
            {
                IdentityId = identityId,
                Name = newOwnerRequest.OwnerName,
                OrganizationId = newOwnerRequest.OrganizationId
            };

            try
            {
                var ownerAdded = await _repository.AddOwner(owner);

                if (newOwnerRequest.RequestId != null)
                {
                    ClientRequest clientRequest = await _repository.GetClientRequest(newOwnerRequest.RequestId ?? default(int));
                    if (clientRequest != null)
                    {

                        clientRequest.OwnerId = ownerAdded.Id;
                        await _repository.UpdateClientRequest(clientRequest);

                        if (clientRequest.AdminNotification != null)
                        {
                            var adminNotification = await _repository.GetAdminNotification(clientRequest.AdminNotification.Id);
                            if (adminNotification != null)
                            {
                                adminNotification.Done = true;
                                await _repository.UpdateAdminNotification(adminNotification);
                            }
                        }

                    }
                }
                return new ServerResponse(StatusCode.Ok);

            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }


        }

    }
}
