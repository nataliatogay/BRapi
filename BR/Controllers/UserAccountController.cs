using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
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
    public class UserAccountController : ResponseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserAccountService _userAccountService;
        private readonly IEmailService _emailService;
        private readonly ISMSConfiguration _smsConfiguration;
        private readonly IMemoryCache _cache;
        private readonly IAsyncRepository _repository;


        public UserAccountController(UserManager<IdentityUser> userManager,
            IUserAccountService userAccountService,
            IEmailService emailService,
            ISMSConfiguration smsConfiguration,
            IMemoryCache cache,
            IAsyncRepository repository)
        {
            _userManager = userManager;
            _userAccountService = userAccountService;
            _emailService = emailService;
            _smsConfiguration = smsConfiguration;
            _cache = cache;
            _repository = repository;
        }


        // if no response type
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



                try
                {
                    // TWILIO


                    TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);

                    var msg = MessageResource.Create(body: code + " is your RB verification code",
                        from: new Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                        to: new Twilio.Types.PhoneNumber(phoneNumber));
                    return new JsonResult(msg.Sid);


                   // return new JsonResult(code);

                }
                catch
                {
                    _cache.Remove(phoneNumber);
                    return new JsonResult("Sending message error");
                }
            }
            else
            {
                return new JsonResult("Code has already been sent");
            }
        }

        [HttpPost("Confirm")]
        public async Task<ActionResult<ServerResponse<LogInUserResponse>>> ConfirmPhone([FromBody]ConfirmPhoneRequest confirmModel)
        {
            string code;
            _cache.TryGetValue(confirmModel.PhoneNumber, out code);
            if (code != null)
            {
                _cache.Remove(confirmModel.PhoneNumber);
                if (confirmModel.Code.Equals(code))
                {
                    var identityUser = await _userManager.FindByNameAsync(confirmModel.PhoneNumber);
                    if (identityUser is null)
                    {
                        var identityResult = await _userManager.CreateAsync(new IdentityUser()
                        {
                            PhoneNumber = confirmModel.PhoneNumber,
                            UserName = confirmModel.PhoneNumber
                        },
                        code);
                        if (identityResult.Succeeded)
                        {
                            identityUser = await _userManager.FindByNameAsync(confirmModel.PhoneNumber);
                        }
                    }
                    return new JsonResult(Response(await _userAccountService.LogIn(identityUser.UserName, identityUser.Id, confirmModel.NotificationTag)));
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





        [Authorize]
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody]NewUserRequest newUserRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);


            if (identityUser != null)
            {
                if ((await _userAccountService.GetInfo(identityUser.Id)) == null)
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
                        _cache.Set(identityUser.Id, newUserRequest.Email, TimeSpan.FromMinutes(3));


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
                        catch
                        {
                            _cache.Remove(identityUser.Id);
                            return new JsonResult("Sending mail error");
                        }
                    }
                    return Ok();
                }
            }
            return new JsonResult("User not found");
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
            if (!_cache.TryGetValue(identityUser.Id, out _))
            {
                _cache.Set(identityUser.Id, newEmail, TimeSpan.FromMinutes(3));
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
                catch
                {
                    _cache.Remove(identityUser.Id);
                    return new JsonResult("Sending mail error");
                }
            }
            else
            {
                return new JsonResult("Link has already been sent");
            }

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new JsonResult("Error");
            }


            //await _userManager.SetEmailAsync(identityUser, newUserRequest.Email);

            string newEmail;
            _cache.TryGetValue(userId, out newEmail);
            if (newEmail != null)
            {
                var identityUser = await _userManager.FindByIdAsync(userId);
                if (identityUser == null)
                {
                    return new JsonResult("Error");
                }
                var result = await _userManager.ConfirmEmailAsync(identityUser, code);
                if (result.Succeeded)
                {
                    identityUser.Email = newEmail;
                    var res = await _userManager.UpdateAsync(identityUser);
                    if (res.Succeeded)
                    {
                        _cache.Remove(identityUser.Id);
                        return Ok();
                    }
                }
            }
            else
            {
                return new JsonResult("Expired");
            }
            return new JsonResult("Error");
        }


        [Authorize]
        [HttpGet("getinfo")]
        public async Task<ActionResult<UserInfoResponse>> GetInfo()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser != null)
            {
                return new JsonResult(await _userAccountService.GetInfo(identityUser.Id));
            }
            return new JsonResult(null);
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody]string notificationTag)
        {
            await _userAccountService.LogOut(notificationTag);
            return Ok();
        }

        [HttpPost("Token")] //api/account/token
        public async Task<IActionResult> UpdateToken([FromBody]string refreshToken)
        {
            LogInResponse resp = await _userAccountService.UpdateToken(refreshToken);
            if (resp is null)
            {
                return StatusCode(401);
            }
            return new JsonResult(resp);
        }


        [HttpPost("UpdateProfile")]
        public async Task<ActionResult<UserInfoResponse>> UpdateProfile([FromBody]UpdateUserRequest updateUserRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            return new JsonResult(await _userAccountService.UpdateProfile(updateUserRequest, identityUser.Id));

        }


    }

}