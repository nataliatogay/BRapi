using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserService _userService;

        public UserController(UserManager<IdentityUser> userManager,
            IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserInfoResponse>>> Get()
        {
            return new JsonResult(await _userService.GetUsers());
        }


        // [Authorize]
        [HttpPost("UploadImage")]
        public async Task<ActionResult<string>> UploadImage([FromBody]string imageString)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult("User not found");
            }
            var path = await _userService.UploadImage(identityUser.Id, imageString);
            return new JsonResult(path);
        }

        [HttpPost("UpdateProfile")]
        public async Task<ActionResult<UserInfoResponse>> UpdateProfile([FromBody]UpdateUserRequest updateUserRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            return new JsonResult(await _userService.UpdateUser(updateUserRequest, identityUser.Id));
            
        }
    }
}