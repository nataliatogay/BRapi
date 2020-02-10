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

        [HttpGet("")]
        public async Task<ActionResult<ServerResponse<ICollection<UserInfoResponse>>>> Get()
        {
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            if (String.IsNullOrEmpty(role))
            {
                return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
            }

            return new JsonResult(Response(await _userService.GetUsers(role)));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<UserInfoResponse>>> Get(int id)
        {
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            if (String.IsNullOrEmpty(role))
            {
                return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
            }
            return new JsonResult(Response(await _userService.GetUser(id, role)));
        }

        [HttpPut("Block")]
        public async Task<ActionResult<ServerResponse>> BlockUser([FromBody]BlockUserRequest blockRequest)
        {
            try
            {
               var res = await _userService.BlockUser(blockRequest);
                if(res is null)
                {
                    return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
                }
            }
            catch
            {
                return new JsonResult(Response(Controllers.StatusCode.Error));
            }
            return new JsonResult(Response(Controllers.StatusCode.Ok));
        }        
    }
}