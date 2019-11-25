using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Sms;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserAccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserAccountService _userAccountService;
        private readonly IEmailService _emailService;
        private readonly ISMSConfiguration _smsConfiguration;
        private readonly IMemoryCache _cache;

        public UserAccountController(UserManager<IdentityUser> userManager,
            IUserAccountService userAccountService,
            IEmailService emailService,
            ISMSConfiguration smsConfiguration,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _userAccountService = userAccountService;
            _emailService = emailService;
            _smsConfiguration = smsConfiguration;
            _cache = cache;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]string phoneNumber)
        {
            var res = await _userManager.FindByNameAsync(phoneNumber);
            if (res != null)
            {
                if (await _userAccountService.UserIsBlocked(res.Id))
                {
                    return new JsonResult("User is blocked");
                }
            }

            if (!_cache.TryGetValue(phoneNumber, out _))
            {
                string code = _userAccountService.GenerateCode();
                _cache.Set(phoneNumber, code, TimeSpan.FromMinutes(3));
                
                
                
                

                //if (res is null)
                //{
                //    IdentityResult identRes = await _userManager.CreateAsync(new IdentityUser()
                //    {
                //        PhoneNumber = phoneNumber,
                //        UserName = phoneNumber
                //    },
                //    code);
                //}
                //else
                //{
                //    if (await _userAccountService.UserIsBlocked(res.Id))
                //    {
                //        return new JsonResult("User is blocked");
                //    }
                //    var token = await _userManager.GeneratePasswordResetTokenAsync(res);
                //    await _userManager.ResetPasswordAsync(res, token, code);
                //}

                // send code
                TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);

                //var msg = MessageResource.Create(body: code + " is your RB verification code",
                //    from: new Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                //    to: new Twilio.Types.PhoneNumber(phoneNumber));

                return new JsonResult(code);

                // return new JsonResult(msg.Sid);
            }
            else
            {
                return new JsonResult("Code has already been sent");
            }
        }

        [HttpPost("Confirm")]
        public async Task<IActionResult> ConfirmPhone([FromBody]ConfirmPhoneRequest confirmModel)
        {
            string code = null;
            _cache.TryGetValue(confirmModel.PhoneNumber, out code);
            if(code != null)
            {
                _cache.Remove(confirmModel.PhoneNumber);
                if (confirmModel.Code.Equals(code))
                {                    
                    var identityUser = await _userManager.FindByNameAsync(confirmModel.PhoneNumber);
                    if(identityUser is null)
                    {
                        var identityResult = await _userManager.CreateAsync(new IdentityUser()
                        {
                            PhoneNumber = confirmModel.PhoneNumber,
                            UserName = confirmModel.PhoneNumber
                        },
                        code);
                        if(identityResult.Succeeded)
                        {
                            identityUser = await _userManager.FindByNameAsync(confirmModel.PhoneNumber);
                        }
                    }
                    return new JsonResult(await _userAccountService.LogIn(identityUser.UserName, identityUser.Id));
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
           // var identityUser = await _userManager.FindByNameAsync(confirmModel.PhoneNumber);
            //if(identityUser != null)
            //{
            //    if (await _userManager.CheckPasswordAsync(identityUser, confirmModel.Code))
            //    {
            //        if (!identityUser.PhoneNumberConfirmed)
            //        {
            //            var token = await _userManager.GenerateChangePhoneNumberTokenAsync(identityUser, identityUser.PhoneNumber);
            //            var result = await _userManager.ChangePhoneNumberAsync(identityUser, identityUser.PhoneNumber, token);
            //        }
            //        // if user is null => новый пользователь. 
            //        return new JsonResult(await _userAccountService.LogIn(identityUser.UserName, identityUser.Id));
            //    } 
            //}           

            //return new JsonResult("Invalid data");
        }





        [Authorize]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]NewUserRequest newUserRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            
            
            if (identityUser != null)
            {
                if((await _userAccountService.GetInfo(identityUser.Id)) == null)
                {
                    User user = new User()
                    {
                        IdentityId = identityUser.Id,
                        FirstName = newUserRequest.FirstName,
                        LastName = newUserRequest.LastName,
                        Gender = newUserRequest.Gender,
                        BirthDate = null
                    };
                    if (newUserRequest.BirthDate != null)   
                    {
                        user.BirthDate = DateTime.ParseExact(newUserRequest.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    await _userAccountService.Register(user);
                    if (newUserRequest.Email != null)
                    {
                        await _userManager.SetEmailAsync(identityUser, newUserRequest.Email);
                        var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                        var callbackUrl = Url.Action(
                                    "ConfirmEmail",
                                    "UserAccount",
                                    new { userId = identityUser.Id, code = emailConfirmationCode },
                                    protocol: HttpContext.Request.Scheme);

                        try
                        {
                            string msgBody = $"<a href='{callbackUrl}'>link</a>";

                            await _emailService.SendAsync(identityUser.Email, "Confirm your email", msgBody);
                            return Ok();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                    return Ok();
                }
                               
            }
            
            return new JsonResult(false);
        }


        [Authorize]
        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody]string newEmail)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult("User not found");
            }

            // add to cookie mail
            //identityUser.Email = newEmail;

            //await _userManager.UpdateAsync(identityUser);

            var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "UserAccount",
                        new { userId = identityUser.Id, code = emailConfirmationCode },
                        protocol: HttpContext.Request.Scheme);

            try
            {
                string msgBody = $"<a href='{callbackUrl}'>link</a>";

                await _emailService.SendAsync(newEmail, "Confirm your email", msgBody);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }

            // после ConfirmEmail -> logIn?? либо перезаписать User.Identity.Name
        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new JsonResult("Error");
            }
            var identityUser = await _userManager.FindByIdAsync(userId);
            if (identityUser == null)
            {
                return new JsonResult("Error");
            }
            
            
            var result = await _userManager.ConfirmEmailAsync(identityUser, code);
            if (result.Succeeded)
            {
                // check cookie
                //identityUser.Email = "newmail@gmail.com";
                var res = await _userManager.UpdateAsync(identityUser);
                if (res.Succeeded)
                {
                    return Ok();
                }
            }
                return new JsonResult("Error");
        }


        [Authorize]
        [HttpGet("getinfo")]
        public async Task<IActionResult> GetInfo()
        { 
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser != null)
            {
                User userAccount = await _userAccountService.GetInfo(identityUser.Id);
                if (userAccount != null)
                {
                    return new JsonResult(userAccount);
                }
            }
            return new JsonResult("User not found");
         }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody]string refreshToken)
        {
            await _userAccountService.LogOut(refreshToken);
            return Ok();
        }
    }
    
}