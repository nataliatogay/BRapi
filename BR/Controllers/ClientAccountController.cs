using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Clients;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAccountController : ResponseController
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClientAccountService _clientAccountService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public ClientAccountController(UserManager<IdentityUser> userManager,
            IClientAccountService clientAccountService,
            IAuthenticationService authenticationService,
            IEmailService emailService,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _clientAccountService = clientAccountService;
            _authenticationService = authenticationService;
            _emailService = emailService;
            _cache = cache;
        }

        [HttpPost("LogIn")]
        public async Task<ActionResult<ServerResponse<LogInResponse>>> LogIn([FromBody]LogInRequest model)
        {
            var identityUser = await _userManager.FindByNameAsync(model.Email);

            if (identityUser != null)
            {
                var role = await _userManager.GetRolesAsync(identityUser);
                if (!role.Contains("Client"))
                {
                    return new JsonResult(Response(Utils.StatusCode.InvalidRole));
                }
                var blockedRes = await _clientAccountService.ClientIsBlocked(identityUser.Id);
                if (blockedRes.StatusCode == Utils.StatusCode.Ok)
                {
                    if (!blockedRes.Data)
                    {
                        var deletedRes = await _clientAccountService.ClientIsDeleted(identityUser.Id);
                        if (deletedRes.StatusCode == Utils.StatusCode.Ok)
                        {
                            if (!deletedRes.Data)
                            {
                                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                                if (checkPassword)
                                {
                                    return new JsonResult(await _clientAccountService.LogIn(identityUser.UserName, model.NotificationTag));
                                }
                            }
                            else
                            {
                                return new JsonResult(Response(Utils.StatusCode.UserDeleted));
                            }
                        }
                        else
                        {
                            return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                        }



                    }
                    else
                    {
                        return new JsonResult(Response(Utils.StatusCode.UserBlocked));
                    }
                }
                else
                {
                    return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                }
            }

            return new JsonResult(Response(Utils.StatusCode.IncorrectLoginOrPassword));
        }

        [Authorize]
        [HttpGet("getinfo")]
        public async Task<IActionResult> GetInfo()
        {

            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser == null)
            {
                return NotFound();
            }

            Client clientAccount = await _clientAccountService.GetInfo(identityUser.Id);
            if (clientAccount is null)
            {
                return NotFound();
            }
            return new JsonResult(clientAccount);
        }


        [Authorize]
        [HttpGet("LogOut/{notificationTag}")]
        public async Task<IActionResult> LogOut(string notificationTag)
        {
            await _clientAccountService.LogOut(notificationTag);
            return Ok();
        }


        
        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]string newPassword)
        {
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


        [Authorize]
        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody]string newEmail)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult("Client not found");
            }


            if (await _userManager.FindByNameAsync(newEmail) is null)
            {
                if (!_cache.TryGetValue(identityUser.Id, out _))
                {
                    _cache.Set(identityUser.Id, newEmail, TimeSpan.FromMinutes(3));

                    var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                    var callbackUrl = Url.Action(
                                "ConfirmEmail",
                                "ClientAccount",
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
                        _cache.Remove(identityUser.Id);
                        throw ex;
                    }
                }
                else
                {
                    return new JsonResult("Link has already been sent");
                }



            }
            else
            {
                return new JsonResult("Email is already used");
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

            string newEmail = null;
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
                    identityUser.UserName = newEmail;
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



        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<ServerResponse>> ForgotPassword([FromBody]string email)
        {

            if (!_cache.TryGetValue(email, out _))
            {
                var user = await _userManager.FindByNameAsync(email);
                if (user == null)
                {
                    return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                }

                var code = _authenticationService.GenerateCode();
                _cache.Set(email, code, TimeSpan.FromMinutes(5));

                try
                {
                    string msgBody = $"Code: {code}";

                    await _emailService.SendAsync(email, "Password reset", msgBody);
                    return new JsonResult(Response(Utils.StatusCode.Ok));
                }
                catch
                {
                    _cache.Remove(email);
                    return new JsonResult(Response(Utils.StatusCode.SendingMailError));
                }

            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.CodeHasAlreadyBeenSent));
            }
        }



        [HttpPost("ResetPassword")]
        [AllowAnonymous]
        //   [ValidateAntiForgeryToken]
        public async Task<ActionResult<ServerResponse>> ResetPassword(ResetPasswordRequest resetRequest)
        {

            string code;
            _cache.TryGetValue(resetRequest.Email, out code);
            if (code != null)
            {
                _cache.Remove(resetRequest.Email);

                if (resetRequest.Code.Equals(code))
                {
                    var user = await _userManager.FindByNameAsync(resetRequest.Email);
                    if (user == null)
                    {
                        return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                    }
                    var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var result = await _userManager.ResetPasswordAsync(user, resetToken, resetRequest.Password);
                    if (result.Succeeded)
                    {
                        return new JsonResult(Response(Utils.StatusCode.Ok));
                    }
                    return new JsonResult(Response(Utils.StatusCode.Error));
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


        [HttpPut("UpdateProfile")]
        public async Task<ActionResult<ServerResponse>> UpdateClientProfile(UpdateClientRequest updateRequest)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientAccountService.UpdateClient(updateRequest, identityUser.Id));
        }



        // by client
        // [Authorize]
        [HttpPost("UploadMainImage")]
        public async Task<ActionResult<ServerResponse<string>>> UploadMainImage([FromBody]string imageString)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientAccountService.UploadMainImage(identityUser.Id, imageString));
        }

        // [Authorize]
        [HttpPut("UploadImages")]
        public async Task<ActionResult<ServerResponse<string>>> UploadImages([FromBody]ICollection<string> imagesString)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientAccountService.UploadImages(identityUser.Id, imagesString));
        }


        // [Authorize]
        [HttpDelete("DeleteImage/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteImage(int id)
        {
            return new JsonResult(await _clientAccountService.DeleteImage(id));
        }

        [HttpPost("Token")]
        public async Task<ActionResult<ServerResponse<LogInResponse>>> UpdateToken([FromBody]string refreshToken)
        {
            return new JsonResult(await _clientAccountService.UpdateToken(refreshToken));
        }


       


    }
}