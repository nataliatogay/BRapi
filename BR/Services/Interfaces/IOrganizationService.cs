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
        Task<ServerResponse<OrganizationInfo>> AddNewOrganization(string title);

        Task<ServerResponse<ICollection<OrganizationInfo>>> GetOrganizations();

        Task<ServerResponse> UpdateOrganizationByOwner(string title, string ownerIdentityId);

        Task<ServerResponse> UpdateUOrganizationByAdmin(UpdateOrganizationRequest updateRequest);

        Task<ServerResponse<string>> UploadLogoByOwner(string imageString, string ownerIdentityId);


        // =============================================================================================

        Task<ServerResponse<OrganizationInfo>> GetOrganization(int id);


        Task<ServerResponse<string>> UploadLogo(UploadOrganizationLogoRequest uploadRequest);

        Task<ServerResponse> DeleteLogo(int organizationId);
    }
}
