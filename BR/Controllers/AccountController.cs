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
    public class AccountController : ControllerBase
    {
        private IAdminAccountService _adminAccountService;
        private readonly UserManager<IdentityUser> _userManager;
        //private readonly SignInManager<IdentityUser> _signInManager;
        private IClientAccountService _clientAccountService;

        public AccountController(IAdminAccountService adminAccountService, 
            UserManager<IdentityUser> userManager,
            IClientAccountService clientAccountService
            /*SignInManager<IdentityUser> signInManager*/)
        {
            _adminAccountService = adminAccountService;
            _userManager = userManager;
            //_signInManager = signInManager;
            _clientAccountService = clientAccountService;
        }

        [HttpPost("Login")] //api/account/login
        public async Task<IActionResult> AdminLogIn([FromBody]LogInRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if(checkPassword)
                {
                    LogInResponse resp = await _adminAccountService.LogIn(model.Email, model.Password);
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
            
            StringValues token;
            HttpContext.Request.Headers.TryGetValue("Authorization", out token);
            var encodedToken = token.ToString().Substring(token.ToString().IndexOf(" ") + 1);
           // var jwt = new JwtSecurityToken(encodedToken);
            
            var handler = new JwtSecurityTokenHandler();
            var jsonToken = handler.ReadToken(encodedToken);
            string userId = ((JwtSecurityToken)jsonToken).Payload["id"].ToString();

            //User. 
            if (userId == null)
            {
                return NotFound();
            }
            Admin adminAccount = await _adminAccountService.GetInfo(Int32.Parse(userId));
            if (adminAccount is null)
            {
                return NotFound();
            }
            return new JsonResult(adminAccount);
        }

       // [Authorize]
        [HttpGet("LogOut")] //api/account/logout
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
            


            return new JsonResult(await _adminAccountService.AddNewAdmin(user));
        }



        [HttpPost("ClientLogin")] 
        public async Task<IActionResult> ClientLogIn([FromBody]LogInRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(user, model.Password);
                if (checkPassword)
                {
                    LogInResponse resp = await _clientAccountService.LogIn(model.Email, model.Password);
                    if (resp != null)
                    {
                        return new JsonResult(resp);
                    }
                }
            }

            return BadRequest();
        }



    }
}