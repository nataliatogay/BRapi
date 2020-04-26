using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO.Organization;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrganizationController : ResponseController
    {
        private readonly IOrganizationService _organizationService;
        private readonly UserManager<IdentityUser> _userManager;

        public OrganizationController(IOrganizationService organizationService,
            UserManager<IdentityUser> userManager)
        {
            _organizationService = organizationService;
            _userManager = userManager;
        }


        [Authorize(Roles ="Admin")]
        [HttpPost("")]
        public async Task<ActionResult<ServerResponse<OrganizationInfo>>> Post([FromBody]string title)
        {
            return new JsonResult(await _organizationService.AddNewOrganization(title));
        }


        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<ServerResponse<ICollection<OrganizationInfo>>>> GetOrganizations()
        {
            return new JsonResult(await _organizationService.GetOrganizations());
        }

        [Authorize(Roles ="Owner")]
        [HttpPut("UpdateByOwner")]
        public async Task<ActionResult<ServerResponse>> EditOrganization([FromBody]string title)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }

            return new ActionResult<ServerResponse>(await _organizationService.UpdateOrganizationByOwner(title, ownerIdentityUser.Id));
        }
         
        [Authorize(Roles ="Admin")]
        [HttpPut("UpdateByAdmin")]
        public async Task<ActionResult<ServerResponse>> EditOrganization([FromBody]UpdateOrganizationRequest updateRequest)
        {
            return new ActionResult<ServerResponse>(await _organizationService.UpdateUOrganizationByAdmin(updateRequest));
        }


        [HttpPut("UploadLogoByOwner")]
        public async Task<ActionResult<ServerResponse<string>>> UploadLogoByOwner([FromBody]string imageString)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _organizationService.UploadLogoByOwner(imageString, ownerIdentityUser.Id));
        }



        // ========================================================================================



        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<OrganizationInfo>>> GetOrganization(int id)
        {
            return new JsonResult(await _organizationService.GetOrganization(id));
        }


        // by owner
        [HttpPost("UploadLogo")]
        public async Task<ActionResult<ServerResponse<string>>> UploadLogo([FromBody]UploadOrganizationLogoRequest uploadRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _organizationService.UploadLogo(uploadRequest));
        }


        // by owner
        [HttpPut("DeleteLogo")]
        public async Task<ActionResult<ServerResponse>> DeleteLogo([FromBody]int organizationId)
        {
            return new JsonResult(await _organizationService.DeleteLogo(organizationId));
        }
    }
}