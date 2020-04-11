using BR.DTO.Organization;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class OrganizationService : IOrganizationService
    {
        private readonly IAsyncRepository _repository;
        private readonly IBlobService _blobService;

        public OrganizationService(IAsyncRepository repository,
            IBlobService blobService)
        {
            _repository = repository;
            _blobService = blobService;
        }

        public async Task<ServerResponse<OrganizationInfoResponse>> AddNewOrganization(string title)
        {
            try
            {
                var org = await _repository.AddOrganization(new Organization()
                {
                    Title = title,
                    LogoPath = "https://rb2020storage.blob.core.windows.net/photos/default-logo.png"
                });
                return new ServerResponse<OrganizationInfoResponse>(StatusCode.Ok, new OrganizationInfoResponse()
                {
                    Id = org.Id,
                    LogoPath = org.LogoPath,
                    Title = org.Title
                });
            }
            catch (DbUpdateException)
            {
                return new ServerResponse<OrganizationInfoResponse>(StatusCode.Duplicate, null);
            }
            catch
            {
                return new ServerResponse<OrganizationInfoResponse>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse<ICollection<OrganizationInfoResponse>>> GetOrganizations()
        {
            ICollection<Organization> organizations;
            try
            {
                organizations = await _repository.GetOrganizations();
                ICollection<OrganizationInfoResponse> res = new List<OrganizationInfoResponse>();
                if (organizations is null)
                {
                    return new ServerResponse<ICollection<OrganizationInfoResponse>>(StatusCode.NotFound, null);
                }
                foreach (var item in organizations)
                {
                    res.Add(new OrganizationInfoResponse()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        LogoPath = item.LogoPath
                    });
                }
                return new ServerResponse<ICollection<OrganizationInfoResponse>>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<ICollection<OrganizationInfoResponse>>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse<OrganizationInfoResponse>> GetOrganization(int id)
        {
            Organization organization;
            try
            {
                organization = await _repository.GetOrganization(id);
                if (organization is null)
                {
                    return new ServerResponse<OrganizationInfoResponse>(StatusCode.NotFound, null);
                }
                return new ServerResponse<OrganizationInfoResponse>(StatusCode.Ok, new OrganizationInfoResponse()
                {
                    Id = organization.Id,
                    Title = organization.Title,
                    LogoPath = organization.LogoPath
                });
            }
            catch
            {
                return new ServerResponse<OrganizationInfoResponse>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse> UpdateUOrganization(UpdateOrganizationRequest updateRequest)
        {
            Organization organization;
            try
            {
                organization = await _repository.GetOrganization(updateRequest.OrganizationId);
                if (organization is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }

            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            organization.Title = updateRequest.Title.Trim();
            try
            {
                await _repository.UpdateOrganization(organization);
                return new ServerResponse(StatusCode.Ok);
            }
            catch (DbUpdateException)
            {
                return new ServerResponse(StatusCode.Duplicate);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }

        public async Task<ServerResponse<string>> UploadLogo(UploadOrganizationLogoRequest uploadRequest)
        {
            Organization organization;
            try
            {
                organization = await _repository.GetOrganization(uploadRequest.OrganizationId);
                if (organization is null)
                {
                    return new ServerResponse<string>(StatusCode.NotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            string imagePath;
            try
            {
                imagePath = await _blobService.UploadImage(uploadRequest.ImageString);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.BlobError, null);
            }
            organization.LogoPath = imagePath;
            try
            {
                await _repository.UpdateOrganization(organization);
                return new ServerResponse<string>(StatusCode.Ok, imagePath);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse> DeleteLogo(int organizationId)
        {
            Organization organization;
            try
            {
                organization = await _repository.GetOrganization(organizationId);
                if (organization is null)
                {
                    return new ServerResponse<string>(StatusCode.NotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            organization.LogoPath = "https://rb2020storage.blob.core.windows.net/photos/default-logo.png";
            try
            {
                await _repository.UpdateOrganization(organization);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }
    }
}
