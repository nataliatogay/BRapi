using BR.DTO.Clients;
using BR.DTO.Organization;
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


        public async Task<ServerResponse<OwnerInfoForOwners>> GetOwnerInfoForOwners(string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<OwnerInfoForOwners>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<OwnerInfoForOwners>(StatusCode.DbConnectionError, null);
            }

            OrganizationInfo organizationInfo = null;
            if (owner.Organization != null)
            {
                organizationInfo = new OrganizationInfo()
                {
                    Id = owner.Organization.Id,
                    Title = owner.Organization.Title,
                    LogoPath = owner.Organization.LogoPath
                };
            }

            return new ServerResponse<OwnerInfoForOwners>(StatusCode.Ok,
                new OwnerInfoForOwners()
                {
                    Name = owner.Name,
                    Email = owner.Identity.Email,
                    PhoneNumber = owner.Identity.PhoneNumber,
                    Organization = organizationInfo
                });
        }


        public async Task<ServerResponse> UpdateOwnerByOwner(UpdateOwnerByOwnerRequest updateRequest, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            owner.Name = updateRequest.Name;
            try
            {
                owner = await _repository.UpdateOwner(owner);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }

    }
}
