using BR.DTO.Organization;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IOrganizationService
    {
        Task<ServerResponse<Organization>> AddNewOrganization(string title);
        
        Task<ServerResponse<ICollection<OrganizationInfoResponse>>> GetOrganizations();

        Task<ServerResponse<OrganizationInfoResponse>> GetOrganization(int id);

        Task<ServerResponse> UpdateUOrganization(UpdateOrganizationRequest updateRequest);

        Task<ServerResponse<string>> UploadLogo(UploadOrganizationLogoRequest uploadRequest);

        Task<ServerResponse> DeleteLogo(int organizationId);
    }
}
