using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Users;
using BR.Models;
using BR.Services;
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
    public class UserController : ResponseController
    {
        private readonly IUserService _userService;
        private readonly UserManager<IdentityUser> _userManager;

        public UserController(UserManager<IdentityUser> userManager,
            IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }


        [HttpGet("ShortForAdmin")]
        public async Task<ActionResult<ServerResponse<ICollection<UserShortInfoForAdmin>>>> ShortForAdmin()
        {
            return new JsonResult(await _userService.GetUserShortInfoForAdmin());
        }

        [Authorize]
        [HttpGet("ForAdmin/{id}")]
        public async Task<ActionResult<ServerResponse<UserInfoForAdmin>>> GetInfoForAdmin(int id)
        {
            return new JsonResult(await _userService.GetUserInfoForAdmin(id));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("GetVisitorsByClient")]
        public async Task<ActionResult<ServerResponse<ICollection<UserFullInfoForClient>>>> GetAllVisitorsByClient()
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _userService.GetAllVisitorsByClient(clientIdentityUser.Id));

        }


        [Authorize(Roles = "Owner")]
        [HttpGet("GetVisitorsByOwner/{clientId}")]
        public async Task<ActionResult<ServerResponse<ICollection<UserShortInfoForClient>>>> GetAllVisitorsByOwner(int clientId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _userService.GetAllVisitorsByOwner(ownerIdentityUser.Id, clientId));
        }


        [Authorize(Roles = "Client")]
        [HttpGet("GetVisitorFullInfoByClient")]
        public async Task<ActionResult<ServerResponse<UserFullInfoForClient>>> GetUserFullInfoByClient(int userId)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _userService.GetUserFullInfoByClient(clientIdentityUser.Id, userId));
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("GetVisitorFullInfoByOwner")]
        public async Task<ActionResult<ServerResponse<UserFullInfoForClient>>> GetUserFullInfoByOwner(int userId, int clientId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _userService.GetUserFullInfoByOwner(ownerIdentityUser.Id, clientId, userId));
        }




        // ====================================================================================



        [HttpPut("Block")]
        public async Task<ActionResult<ServerResponse>> BlockUser([FromBody]int userId)
        {
            return new JsonResult(await _userService.BlockUser(userId));
        }

        [HttpPut("Unblock")]
        public async Task<ActionResult<ServerResponse>> UnblockUser([FromBody]int userId)
        {
            return new JsonResult(await _userService.UnblockUser(userId));
        }



        [HttpGet("ForUsers/{id}")]
        public async Task<ActionResult<ServerResponse<UserInfoForUsers>>> GetInfoForUser(int id)
        {
            return new JsonResult(await _userService.GetUserInfoForUsers(id));
        }





    }
}