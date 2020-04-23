using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO.Organization;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
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

        [HttpPost("")]
        public async Task<ActionResult<ServerResponse<OrganizationInfo>>> Post([FromBody]string title)
        {
            return new JsonResult(await _organizationService.AddNewOrganization(title));
        }

        [HttpGet]
        public async Task<ActionResult<ServerResponse<ICollection<OrganizationInfo>>>> GetOrganizations()
        {
            return new JsonResult(await _organizationService.GetOrganizations());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<OrganizationInfo>>> GetOrganization(int id)
        {
            return new JsonResult(await _organizationService.GetOrganization(id));
        }

        //by owner
        [HttpPut]
        public async Task<ActionResult<ServerResponse>> EditOrganization([FromBody]UpdateOrganizationRequest updateRequest)
        {
            return new ActionResult<ServerResponse>(await _organizationService.UpdateUOrganization(updateRequest));
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