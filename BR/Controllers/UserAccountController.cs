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

//upload image

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
        }


        [HttpPost("Login")]
        public async Task<ActionResult<ServerResponse>> Login([FromBody]string phoneNumber)
        {
            var res = await _userManager.FindByNameAsync(phoneNumber);
            if (res != null)
            {
                if (await _userAccountService.UserIsBlocked(res.Id))
                {
                    return Response(Controllers.StatusCode.UserBlocked);
                    //   return new JsonResult(Response(Controllers.StatusCode.UserBlocked));
                }
            }

            if (!_cache.TryGetValue(phoneNumber, out _))
            {
                string code = _userAccountService.GenerateCode();
                _cache.Set(phoneNumber, code, TimeSpan.FromMinutes(3));



                try
                {
                    // TWILIO


                    //TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);

                    //var msg = MessageResource.Create(body: code + " is your RB verification code",
                    //  from: new Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                    //  to: new Twilio.Types.PhoneNumber(phoneNumber));
                    //return new JsonResult(Response(Controllers.StatusCode.Ok));
                    //return new JsonResult(msg.Sid);


                    return new JsonResult(code);

                }
                catch
                {
                    _cache.Remove(phoneNumber);
                    return new JsonResult(Response(Controllers.StatusCode.SendingMessageError));
                }
            }
            else
            {
                return new JsonResult(Response(Controllers.StatusCode.CodeHasAlreadyBeenSent));
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
                    var resp = await _userAccountService.LogIn(identityUser.UserName, identityUser.Id, confirmModel.NotificationTag);
                    return new JsonResult(Response(resp));
                }
                else
                {
                    return new JsonResult(Response(Controllers.StatusCode.IncorrectVerificationCode));
                }
            }
            else
            {
                return new JsonResult(Response(Controllers.StatusCode.Expired));
            }
        }




        // email confirm

        [Authorize]
        [HttpPost("Register")]
        public async Task<ActionResult<ServerResponse>> Register([FromBody]NewUserRequest newUserRequest)
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
                            await _emailService.SendAsync(newUserRequest.Email, "Confirm your email", msgBody);
                            return new JsonResult(Response(Controllers.StatusCode.Ok));
                        }
                        catch
                        {
                            _cache.Remove(identityUser.Id);
                            return new JsonResult(Response(Controllers.StatusCode.SendingMailError));
                        }
                    }
                    return new JsonResult(Response(Controllers.StatusCode.Ok));
                }
                else
                {
                    return new JsonResult(Response(Controllers.StatusCode.UserRegistered));
                }
            }
            return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
        }


        [Authorize]
        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody]string newEmail)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
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
                    return new JsonResult(Response(Controllers.StatusCode.Ok));
                }
                catch
                {
                    _cache.Remove(identityUser.Id);
                    return new JsonResult(Response(Controllers.StatusCode.SendingMailError));
                }
            }
            else
            {
                return new JsonResult(Response(Controllers.StatusCode.LinkHasAlreadyBeenSent));
            }

        }


        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new JsonResult(Response(Controllers.StatusCode.Error));
            }


            //await _userManager.SetEmailAsync(identityUser, newUserRequest.Email);

            string newEmail;
            _cache.TryGetValue(userId, out newEmail);
            if (newEmail != null)
            {
                var identityUser = await _userManager.FindByIdAsync(userId);
                if (identityUser == null)
                {
                    return new JsonResult(Response(Controllers.StatusCode.Error));
                }
                var result = await _userManager.ConfirmEmailAsync(identityUser, code);
                if (result.Succeeded)
                {
                    identityUser.Email = newEmail;
                    var res = await _userManager.UpdateAsync(identityUser);
                    if (res.Succeeded)
                    {
                        _cache.Remove(identityUser.Id);
                        return new JsonResult(Response(Controllers.StatusCode.Ok));
                    }
                }
            }
            else
            {
                return new JsonResult(Response(Controllers.StatusCode.Expired));
            }
            return new JsonResult(Response(Controllers.StatusCode.Error));
        }


        [Authorize]
        [HttpGet("getinfo")]
        public async Task<ActionResult<ServerResponse<UserInfoResponse>>> GetInfo()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser != null)
            {
                var user = await _userAccountService.GetInfo(identityUser.Id);
                if (user != null)
                {
                    return new JsonResult(Response(user));
                }
                return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
            }
            return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
        }

        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody]string notificationTag)
        {
            await _userAccountService.LogOut(notificationTag);
            return new JsonResult(Response(Controllers.StatusCode.Ok));
        }

        [HttpPost("Token")] //api/account/token
        public async Task<ActionResult<ServerResponse>> UpdateToken([FromBody]string refreshToken)
        {
            LogInResponse resp = await _userAccountService.UpdateToken(refreshToken);
            if (resp is null)
            {
                return StatusCode(401);
            }
            return new JsonResult(Response(resp));
        }


        // return void
        [Authorize]
        [HttpPost("UpdateProfile")]
        public async Task<ActionResult<ServerResponse<UserInfoResponse>>> UpdateProfile([FromBody]UpdateUserRequest updateUserRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
            }
            var res = await _userAccountService.UpdateProfile(updateUserRequest, identityUser.Id);
            return new JsonResult(Response(res));

        }


        [Authorize]
        [HttpPost("UploadImage")]
        public async Task<ActionResult<ServerResponse<string>>> UploadImage([FromBody]string imageString)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
            }
            try
            {
                var res = await _userAccountService.UploadImage(identityUser.Id, imageString);
            return new JsonResult(Response(res));
            }
            catch
            {
                return new JsonResult(Response(Controllers.StatusCode.Error));
            }
        }

        [HttpDelete]
        public async Task<IActionResult> Delete()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Controllers.StatusCode.UserNotFound));
            }
            try
            {
                var res = await _userAccountService.DeleteUser(identityUser.Id);
                if (res)
                {
                    var resDel = await _userManager.DeleteAsync(identityUser);
                    if (resDel.Succeeded)
                    {
                        return new JsonResult(Response(Controllers.StatusCode.Ok));
                    }
                }
                return new JsonResult(Response(Controllers.StatusCode.Error));
            }
            catch
            {
                return new JsonResult(Response(Controllers.StatusCode.Error));
            }
        }
    }

}