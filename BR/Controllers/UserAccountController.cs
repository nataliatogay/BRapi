using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserAccountService _userAccountService;

        [HttpPost("Register")]
        public async Task Register([FromBody]string phoneNumber)
        {
            IdentityUser user = new IdentityUser()
            {
                PhoneNumber = phoneNumber,
                UserName = phoneNumber
            };
           
            // check phone or username
            // generate code
            
            IdentityResult res = await _userManager.CreateAsync(user, "admin");

            //user = await _userManager.FindByNameAsync(adminEmail);



            // return new JsonResult(await _adminAccountService.AddNewAdmin(user));
           // return Ok();
        }



        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody]LogInUserRequest model)
        {
            var identityUser = await _userManager.FindByNameAsync(model.PhoneNumber);
            if (identityUser != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Code);
                if (checkPassword)
                {
                    LogInResponse resp = await _userAccountService.LogIn(identityUser);
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