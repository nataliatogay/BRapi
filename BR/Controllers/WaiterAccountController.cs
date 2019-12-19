using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.Services;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Twilio;
using Twilio.Rest.Api.V2010.Account;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WaiterAccountController : ControllerBase
    {
        private readonly IWaiterAccountService _waiterAccountService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISMSConfiguration _smsConfiguration;
        private readonly IMemoryCache _cache;
         
        public WaiterAccountController(IWaiterAccountService waiterAccountService,
            UserManager<IdentityUser> userManager,
            ISMSConfiguration smsConfiguration,
            IMemoryCache cache)
        {
            _waiterAccountService = waiterAccountService;
            _userManager = userManager;
            _smsConfiguration = smsConfiguration;
            _cache = cache;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn([FromBody]LogInWaiterRequest model)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(model.Login);
            if (identityUser != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                if (checkPassword)
                {
                    LogInResponse resp = await _waiterAccountService.LogIn(identityUser.UserName, identityUser.Id);
                    if (resp != null)
                    {
                        return new JsonResult(resp);
                    }
                }
            }

            return new JsonResult(null);
        }

        //  [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody]string refreshToken)
        {
            await _waiterAccountService.LogOut(refreshToken);
            return Ok();
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]string newPassword)
        {
            // send sms code
            IdentityUser user = await _userManager.FindByNameAsync(User.Identity.Name);
            if (user != null)
            {
                var _passwordValidator =
                    HttpContext.RequestServices.GetService(typeof(IPasswordValidator<IdentityUser>)) as IPasswordValidator<IdentityUser>;
                var _passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<IdentityUser>)) as IPasswordHasher<IdentityUser>;

                IdentityResult result =
                    await _passwordValidator.ValidateAsync(_userManager, user, newPassword);
                if (result.Succeeded)
                {
                    user.PasswordHash = _passwordHasher.HashPassword(user, newPassword);
                    await _userManager.UpdateAsync(user);
                    return Ok();
                }
                else
                {
                    StringBuilder errors = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        errors.Append(error);
                    }
                }
            }
            return new JsonResult("Client not found");
        }


        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        //  [ValidateAntiForgeryToken] -> BadRequest
        public async Task<IActionResult> ForgotPassword([FromBody]string login)
        {
            var identityUser = await _userManager.FindByNameAsync(login);
            if (identityUser == null)
            {
                return new JsonResult("Waiter not found");
            }
            else
            {
                if (!_cache.TryGetValue(identityUser.NormalizedUserName, out _))
                {
                    //var code = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                    var code = await _userManager.GenerateChangePhoneNumberTokenAsync(identityUser, identityUser.PhoneNumber);
                    _cache.Set(identityUser.NormalizedUserName, code, TimeSpan.FromMinutes(3));


                    try
                    {
                        // TWILIO

                        /*
                        TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);

                        var msg = MessageResource.Create(body: code + " is your RB verification code",
                            from: new  Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                            to: new Twilio.Types.PhoneNumber(identityUser.PhoneNumber));
                        return new JsonResult(msg.Sid);
                        */

                        return new JsonResult(code);
                    }
                    catch
                    {
                        _cache.Remove(identityUser.NormalizedUserName);
                        return new JsonResult("Sending message error");
                    }
                }
                else
                {
                    return new JsonResult("Code has already been sent");
                }
            }
        }



        [HttpPost("Confirm")]
        public async Task<IActionResult> ResetPassword([FromBody]ResetPasswordWaiterRequest resetModel)
        {
            string code;
            _cache.TryGetValue(resetModel.Login.ToUpper(), out code);
            if (code != null)
            {
                _cache.Remove(resetModel.Login.ToUpper());
                if (resetModel.Code.Equals(code))
                {
                    var identityUser = await _userManager.FindByNameAsync(resetModel.Login);
                    if (identityUser is null)
                    {
                        return new JsonResult("Waiter not found");
                    } 
                    else
                    {
                        var resetCode = await _userManager.GeneratePasswordResetTokenAsync(identityUser);
                        var result = await _userManager.ResetPasswordAsync(identityUser, resetCode, resetModel.Password);
                        if (result.Succeeded)
                        {
                            return Ok();
                        }
                        return new JsonResult("Error");
                    }
                }
                else
                {
                    return new JsonResult("Incorrect code");
                }
            }
            else
            {
                return new JsonResult("Expired");
            }
        }
    }
}