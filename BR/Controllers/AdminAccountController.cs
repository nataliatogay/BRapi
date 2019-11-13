using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private readonly IAdminAccountService _adminAccountService;
        private readonly UserManager<IdentityUser> _userManager;

        public AdminAccountController(IAdminAccountService adminAccountService, 
            UserManager<IdentityUser> userManager)
        {
            _adminAccountService = adminAccountService;
            _userManager = userManager;
        }

        [HttpPost("Login")] 
        public async Task<IActionResult> LogIn([FromBody]LogInRequest model)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(model.Email);
            if (identityUser != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                if(checkPassword)
                {
                    LogInResponse resp = await _adminAccountService.LogIn(identityUser.UserName, identityUser.Id);
                    if (resp != null)
                    {
                        return new JsonResult(resp);
                    } 
                }
            }

            return BadRequest();
        }

        [HttpPost("Token")] //api/account/token
        public async Task<IActionResult> UpdateToken([FromBody]string refreshToken)
        {
            LogInResponse resp = await _adminAccountService.UpdateToken(refreshToken);
            if (resp is null)
            {
                return StatusCode(401);
            }
            return new JsonResult(resp);
        }

        [Authorize]
        [HttpGet("getinfo")] 
        public async Task<IActionResult> GetInfo()
        { // claim based policy
                       
            
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if(identityUser == null)
            {
                return NotFound();
            }

            Admin adminAccount = await _adminAccountService.GetInfo(identityUser.Id);
            if (adminAccount is null)
            {
                return NotFound();
            }
            return new JsonResult(adminAccount);
        }

        [Authorize]
        [HttpGet("LogOut")] 
        public async Task<IActionResult> LogOut(string refreshToken)
        {

           // string id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            //if (id == null)
            //{
            //    return new JsonResult("no admin");
            //}
            await _adminAccountService.LogOut(refreshToken);

            return Ok();
            //return new JsonResult(_accountService.LogOut(Int32.Parse(id)));
            //return Ok();
        }

        
        [HttpPost("AdminRegister")]
        public async Task<IActionResult> AdminRegister([FromBody]string adminEmail)
        {
            IdentityUser user = new IdentityUser()
            {
                Email = adminEmail,
                UserName = adminEmail  
            };
            IdentityResult res = await _userManager.CreateAsync(user, "admin");

            user = await _userManager.FindByNameAsync(adminEmail);
            Admin admin = new Admin()
            {
                IdentityId = user.Id
            };
            


            return new JsonResult(await _adminAccountService.AddNewAdmin(admin));
        }



        



    }
}