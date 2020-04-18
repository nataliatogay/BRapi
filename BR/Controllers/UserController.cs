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

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpGet("ShortForAdmin")]
        public async Task<ActionResult<ServerResponse<ICollection<UserShortInfoForAdminResponse>>>> ShortForAdmin()
        {
            return new JsonResult(await _userService.GetUserShortInfoForAdmin());
        }

        [HttpGet("ForUsers/{id}")]
        public async Task<ActionResult<ServerResponse<UserInfoForUsersResponse>>> GetInfoForUser(int id)
        {
            return new JsonResult(await _userService.GetUserInfoForUsers(id));
        }

        [Authorize]
        [HttpGet("ForAdmin/{id}")]
        public async Task<ActionResult<ServerResponse<UserInfoForAdminResponse>>> GetInfoForAdmin(int id)
        {
            return new JsonResult(await _userService.GetUserInfoForAdmin(id));
        }

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

    }
}