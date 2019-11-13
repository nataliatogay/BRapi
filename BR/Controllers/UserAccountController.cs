using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Authorization;
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

        public UserAccountController(UserManager<IdentityUser> userManager,
            IUserAccountService userAccountService)
        {
            _userManager = userManager;
            _userAccountService = userAccountService;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]string phoneNumber)
        {

            var res = await _userManager.FindByNameAsync(phoneNumber);
            if (res is null || !res.PhoneNumberConfirmed)
            {
                string code = _userAccountService.GenerateCode();

                // send code

                if (res is null)
                {
                    IdentityResult identRes = await _userManager.CreateAsync(new IdentityUser()
                    {
                        PhoneNumber = phoneNumber,
                        UserName = phoneNumber
                    },
                    code);
                }
                else
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(res);
                    await _userManager.ResetPasswordAsync(res, token, code);
                }
                return new JsonResult(code);
            }

            return BadRequest();
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmPhone([FromBody]ConfirmPhoneRequest confirmModel)
        {
            var identityUser = await _userManager.FindByNameAsync(confirmModel.PhoneNumber);
            if(identityUser != null)
            {
                if (await _userManager.CheckPasswordAsync(identityUser, confirmModel.Code))
                {
                    if(!identityUser.PhoneNumberConfirmed)
                    {
                       // identityUser.PhoneNumberConfirmed = true;
                        var token = await _userManager.GenerateChangePhoneNumberTokenAsync(identityUser, identityUser.PhoneNumber);
                        var result = await _userManager.ChangePhoneNumberAsync(identityUser, identityUser.PhoneNumber, token);
                        
                        await _userAccountService.Register(new Models.User() { IdentityId = identityUser.Id });
                    }
                    return new JsonResult(await _userAccountService.LogIn(identityUser.UserName, identityUser.Id));
                } 
            }
            

            return Ok("Invalid data");
        }





        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody]string phoneNumber)
        {
            var identityUser = await _userManager.FindByNameAsync(phoneNumber);
            if (identityUser != null && identityUser.PhoneNumberConfirmed)
            {
                string code = _userAccountService.GenerateCode();

                //send code

                var token = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                await _userManager.ResetPasswordAsync(identityUser, token, code);

                return new JsonResult(code);                
            }

            return new JsonResult("User is not registered");
        }

        [Authorize]
        [HttpGet("getinfo")]
        public async Task<IActionResult> GetInfo()
        { // claim based policy


            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser == null)
            {
                return NotFound();
            }

            User userAccount = await _userAccountService.GetInfo(identityUser.Id);
            if (userAccount is null)
            {
                return new JsonResult("User not found");
            }
            return new JsonResult(userAccount);
        }
    }

    
}