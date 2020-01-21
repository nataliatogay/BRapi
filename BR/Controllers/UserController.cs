using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
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
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<UserInfoResponse>>> Get()
        {
            return new JsonResult(await _userService.GetUsers());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<UserInfoResponse>> Get(int id)
        {
            return new JsonResult(await _userService.GetUser(id));
        }

        [HttpPut("Block")]
        public async Task<ActionResult<User>> BlockUser(int id)
        {
            return new JsonResult(await _userService.BlockUser(id));
        }        
    }
}