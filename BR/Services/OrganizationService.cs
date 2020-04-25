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

        public async Task<ServerResponse<OrganizationInfo>> AddNewOrganization(string title)
        {
            try
            {
                var org = await _repository.AddOrganization(new Organization()
                {
                    Title = title,
                    LogoPath = "https://rb2020storage.blob.core.windows.net/photos/default-logo.png"
                });
                return new ServerResponse<OrganizationInfo>(StatusCode.Ok, new OrganizationInfo()
                {
                    Id = org.Id,
                    LogoPath = org.LogoPath,
                    Title = org.Title
                });
            }
            catch (DbUpdateException)
            {
                return new ServerResponse<OrganizationInfo>(StatusCode.Duplicate, null);
            }
            catch
            {
                return new ServerResponse<OrganizationInfo>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse<ICollection<OrganizationInfo>>> GetOrganizations()
        {
            ICollection<Organization> organizations;
            try
            {
                organizations = await _repository.GetOrganizations();
                ICollection<OrganizationInfo> res = new List<OrganizationInfo>();
                if (organizations is null)
                {
                    return new ServerResponse<ICollection<OrganizationInfo>>(StatusCode.NotFound, null);
                }
                foreach (var item in organizations)
                {
                    res.Add(new OrganizationInfo()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        LogoPath = item.LogoPath
                    });
                }
                return new ServerResponse<ICollection<OrganizationInfo>>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<ICollection<OrganizationInfo>>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse> UpdateOrganizationByOwner(string title, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
                if (owner.Organization is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            var organization = owner.Organization;
            organization.Title = title;

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


        public async Task<ServerResponse> UpdateUOrganizationByAdmin(UpdateOrganizationRequest updateRequest)
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







        // =====================================================================

        public async Task<ServerResponse<OrganizationInfo>> GetOrganization(int id)
        {
            Organization organization;
            try
            {
                organization = await _repository.GetOrganization(id);
                if (organization is null)
                {
                    return new ServerResponse<OrganizationInfo>(StatusCode.NotFound, null);
                }
                return new ServerResponse<OrganizationInfo>(StatusCode.Ok, new OrganizationInfo()
                {
                    Id = organization.Id,
                    Title = organization.Title,
                    LogoPath = organization.LogoPath
                });
            }
            catch
            {
                return new ServerResponse<OrganizationInfo>(StatusCode.DbConnectionError, null);
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
