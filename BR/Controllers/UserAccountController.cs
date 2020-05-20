using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Users;
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
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IUserAccountService _userAccountService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly ISMSConfiguration _smsConfiguration;
        private readonly IMemoryCache _cache;


        public UserAccountController(UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IUserAccountService userAccountService,
            IAuthenticationService authenticationService,
            IEmailService emailService,
            ISMSConfiguration smsConfiguration,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _userAccountService = userAccountService;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _smsConfiguration = smsConfiguration;
            _cache = cache;
        }


        // DONE
        [HttpPost("Login")]
        public async Task<ActionResult<ServerResponse>> Login([FromBody]string phoneNumber)
        {
            if (!_cache.TryGetValue(phoneNumber, out _))
            {
                string code = _authenticationService.GenerateCode();
                _cache.Set(phoneNumber, code, TimeSpan.FromMinutes(3));

                try
                {
                    // TWILIO


                    //TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);

                    //var msg = MessageResource.Create(body: code + " is your RB verification code",
                    //from: new Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                    //to: new Twilio.Types.PhoneNumber(phoneNumber));
                    //string sid = msg.Sid;
                    //return new JsonResult(Response(Utils.StatusCode.Ok));
                    //return new JsonResult(msg.Sid);
                    return new JsonResult(code);

                }
                catch
                {
                    _cache.Remove(phoneNumber);
                    return new JsonResult(Response(Utils.StatusCode.SendingMessageError));
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.CodeHasAlreadyBeenSent));
            }

        }


        //DONE
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
                    bool isDeleted = false;
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
                            var res = await _userManager.AddToRoleAsync(identityUser, "User");
                            if (!res.Succeeded)
                            {
                                return new JsonResult(Response(Utils.StatusCode.Error));
                            }

                        }
                        else
                        {
                            return new JsonResult(Response(Utils.StatusCode.Error));
                        }
                    }
                    else
                    {

                        var blockedRes = await _userAccountService.UserIsBlocked(identityUser.Id);
                        if (blockedRes.StatusCode == Utils.StatusCode.Ok && blockedRes.Data)
                        {
                            return new JsonResult(Response(Utils.StatusCode.UserBlocked));
                        }
                        var deletedRes = await _userAccountService.UserIsDeleted(identityUser.Id);
                        if (deletedRes.StatusCode == Utils.StatusCode.Ok && deletedRes.Data)
                        {
                            // делать log In, отправлять токены и спросить, желает ли восстановить данные 
                            isDeleted = true;
                        }
                    }
                    var resp = await _userAccountService.LogIn(identityUser.UserName, identityUser.Id, confirmModel.NotificationTag);
                    if (resp.StatusCode == Utils.StatusCode.Ok && isDeleted)
                    {
                        resp.StatusCode = Utils.StatusCode.UserDeleted;
                    }
                    return new JsonResult(resp);
                }
                else
                {
                    return new JsonResult(Response(Utils.StatusCode.IncorrectVerificationCode));
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.Expired));
            }
        }





        // DONE
        [Authorize(Roles = "User")]
        [HttpPost("Register")]
        public async Task<ActionResult<ServerResponse<UserInfoForUsers>>> Register([FromBody]NewUserRequest newUserRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);

            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            var isRegistered = await _userAccountService.UserIsRegistered(identityUser.Id);

            if (isRegistered.StatusCode != Utils.StatusCode.Ok)
            {
                return new JsonResult(Response(isRegistered.StatusCode));
            }

            if (!isRegistered.Data)
            {

                return new JsonResult(await _userAccountService.Register(newUserRequest, identityUser.Id));

            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.UserRegistered));
            }
        }


        [Authorize]
        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody]string newEmail)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
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
                    return new JsonResult(Response(Utils.StatusCode.Ok));
                }
                catch
                {
                    _cache.Remove(identityUser.Id);
                    return new JsonResult(Response(Utils.StatusCode.SendingMailError));
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.LinkHasAlreadyBeenSent));
            }

        }


        // DONE
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return new JsonResult(Response(Utils.StatusCode.Error));
            }


            //await _userManager.SetEmailAsync(identityUser, newUserRequest.Email);

            string newEmail;
            _cache.TryGetValue(userId, out newEmail);
            if (newEmail != null)
            {
                var identityUser = await _userManager.FindByIdAsync(userId);
                if (identityUser == null)
                {
                    return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                }
                else
                {
                    var result = await _userManager.ConfirmEmailAsync(identityUser, code);
                    if (result.Succeeded)
                    {
                        identityUser.Email = newEmail;
                        var res = await _userManager.UpdateAsync(identityUser);
                        if (res.Succeeded)
                        {
                            _cache.Remove(identityUser.Id);

                            return new JsonResult(Response(Utils.StatusCode.Ok));
                        }
                    }
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.Expired));
            }
            return new JsonResult(Response(Utils.StatusCode.Error));
        }



        [HttpPost("LogOut")]
        public async Task<ActionResult<ServerResponse>> LogOut([FromBody]string notificationTag)
        {
            await _userAccountService.LogOut(notificationTag);
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }



        // DONE
        [HttpPost("Token")] //api/account/token
        public async Task<ActionResult<ServerResponse<LogInResponse>>> UpdateToken([FromBody]string refreshToken)
        {
            return new JsonResult(await _userAccountService.UpdateToken(refreshToken));
        }



        [Authorize(Roles = "User")]
        [HttpPost("UpdateProfile")]
        public async Task<ActionResult<ServerResponse>> UpdateProfile([FromBody]UpdateUserRequest updateUserRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _userAccountService.UpdateProfile(updateUserRequest, identityUser.Id));


        }


        [Authorize(Roles = "User")]
        [HttpPost("UploadImage")]
        public async Task<ActionResult<ServerResponse<string>>> UploadImage([FromBody]string imageString)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            try
            {
                return new JsonResult(await _userAccountService.UploadImage(identityUser.Id, imageString));
            }
            catch
            {
                return new JsonResult(Response(Utils.StatusCode.Error));
            }
        }

        [Authorize(Roles = "User")]
        [HttpPost("DeleteImage")]
        public async Task<ActionResult<ServerResponse>> DeleteImage()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _userAccountService.DeleteImage(identityUser.Id));
        }



        // ================================================================================


        [HttpPut]
        public async Task<ActionResult<ServerResponse>> Delete()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _userAccountService.DeleteUser(identityUser.Id));
        }

        [Authorize]
        [HttpPut("Restore")]
        public async Task<ActionResult<ServerResponse>> RestoreDeletedUser()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser != null)
            {
                return new JsonResult(await _userAccountService.RestoreUser(identityUser.Id));
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
        }


        [Authorize]
        [HttpPut("FinallyDelete")]
        public async Task<ActionResult<ServerResponse<LogInUserResponse>>> FinallyDelete(string notificationTag)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            var userPhone = identityUser.UserName;


            // check unique
            var newName = identityUser.UserName + "_DELETED_" + DateTime.Now.ToString();
            identityUser.UserName = newName;

            var changeRes = await _userManager.UpdateAsync(identityUser);
            if (!changeRes.Succeeded)
            {
                return new JsonResult(Response(Utils.StatusCode.Error));
            }
            var newUserRes = await _userManager.CreateAsync(new IdentityUser()
            {
                PhoneNumber = userPhone,
                UserName = userPhone
            },
            "1234");
            if (!newUserRes.Succeeded)
            {
                identityUser.UserName = userPhone;
                await _userManager.UpdateAsync(identityUser);
                return new JsonResult(Response(Utils.StatusCode.Error));
            }
            else
            {
                var newIdentityId = await _userManager.FindByNameAsync(userPhone);
                if (newIdentityId is null)
                {
                    return new JsonResult(Response(Utils.StatusCode.Error));
                }
                var roleRes = await _userManager.AddToRoleAsync(newIdentityId, "User");
                if (!roleRes.Succeeded)
                {
                    return new JsonResult(Response(Utils.StatusCode.Error));
                }
                await _userAccountService.FinallyDelete(notificationTag);
                return new JsonResult(await _userAccountService.LogIn(userPhone, newIdentityId.Id, notificationTag));
            }
        }
    }

}