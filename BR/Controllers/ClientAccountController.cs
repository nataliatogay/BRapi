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
        public async Task<ActionResult<ServerResponse<LogInClientOwnerResponse>>> LogIn([FromBody]LogInRequest model)
        {
            var identityUser = await _userManager.FindByNameAsync(model.Email);

            if (identityUser != null)
            {
                var role = await _userManager.GetRolesAsync(identityUser);
                if (!role.Contains("Client") && !role.Contains("Owner"))
                {
                    return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(Utils.StatusCode.InvalidRole, null));
                }
                if (role.Contains("Client"))
                {
                    var blockedRes = await _clientAccountService.ClientIsBlocked(identityUser.Id);
                    if (blockedRes.StatusCode != Utils.StatusCode.Ok)
                    {
                        return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(blockedRes.StatusCode, null));
                    }

                    if (blockedRes.Data)
                    {
                        return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(Utils.StatusCode.UserBlocked, null));
                    }


                    var deletedRes = await _clientAccountService.ClientIsDeleted(identityUser.Id);
                    if (deletedRes.StatusCode != Utils.StatusCode.Ok)
                    {

                        return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(deletedRes.StatusCode, null));
                    }
                    if (deletedRes.Data)
                    {
                        return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(Utils.StatusCode.UserDeleted, null));
                    }


                }

                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                if (checkPassword)
                {
                    var loginRes = await _clientAccountService.LogIn(identityUser.UserName, model.NotificationTag);
                    if (loginRes.StatusCode != Utils.StatusCode.Ok)
                    {
                        return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(loginRes.StatusCode, null));
                    }
                    return new JsonResult(Response(Utils.StatusCode.Ok, new LogInClientOwnerResponse()
                    {
                        AccessToken = loginRes.Data.AccessToken,
                        RefreshToken = loginRes.Data.RefreshToken,
                        Role = role[0]
                    }));

                }
                else
                {
                    return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(Utils.StatusCode.IncorrectLoginOrPassword, null));
                }


            }
            else
            {
                return new JsonResult(new ServerResponse<LogInClientOwnerResponse>(Utils.StatusCode.UserNotFound, null));
            }
        }


        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<ServerResponse>> ForgotPassword([FromBody]string email)
        {

            if (!_cache.TryGetValue(email, out _))
            {
                var identityUser = await _userManager.FindByNameAsync(email);
                if (identityUser is null)
                {
                    return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                }
                var roles = await _userManager.GetRolesAsync(identityUser);
                if (!roles.Contains("Client") && !roles.Contains("Owner"))
                {
                    return new JsonResult(Response(Utils.StatusCode.InvalidRole));
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



        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<ActionResult<ServerResponse>> ChangePassword([FromBody]string newPassword)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser != null)
            {
                var _passwordValidator =
                    HttpContext.RequestServices.GetService(typeof(IPasswordValidator<IdentityUser>)) as IPasswordValidator<IdentityUser>;
                var _passwordHasher =
                    HttpContext.RequestServices.GetService(typeof(IPasswordHasher<IdentityUser>)) as IPasswordHasher<IdentityUser>;

                IdentityResult result =
                    await _passwordValidator.ValidateAsync(_userManager, identityUser, newPassword);
                if (result.Succeeded)
                {
                    identityUser.PasswordHash = _passwordHasher.HashPassword(identityUser, newPassword);
                    var updateResult = await _userManager.UpdateAsync(identityUser);
                    if (updateResult.Succeeded)
                    {
                        return new JsonResult(Response(Utils.StatusCode.Ok));
                    }
                }
                return new JsonResult(Response(Utils.StatusCode.Error));
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
        }



        [Authorize(Roles = "Owner")]
        [Authorize(Roles = "Client")]
        [HttpPost("ChangeEmail")]
        public async Task<ActionResult<ServerResponse>> ChangeEmail([FromBody]string newEmail)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            if (await _userManager.FindByNameAsync(newEmail) is null)
            {
                _cache.Set(identityUser.Id, newEmail, TimeSpan.FromMinutes(3));

                var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
                var callbackUrl = Url.Action(
                            "ConfirmEmail",
                            "AdminAccount",
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
                    return new JsonResult(Response(Utils.StatusCode.Error));
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.EmailUsed));
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


        [HttpPost("Token")]
        public async Task<ActionResult<ServerResponse<LogInResponse>>> UpdateToken([FromBody]string refreshToken)
        {
            return new JsonResult(await _clientAccountService.UpdateToken(refreshToken));
        }



        [Authorize]
        [HttpPost("LogOut")]
        public async Task<ActionResult<ServerResponse>> LogOut([FromBody]string notificationTag)
        {
            try
            {
                await _clientAccountService.LogOut(notificationTag);
            }
            catch { }
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }



        [Authorize(Roles = "Client")]
        [HttpGet("ProfileInfo")]
        public async Task<ActionResult<ServerResponse<ClientFullInfoForClients>>> GetInfo()
        {

            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser == null)
            {
                return new JsonResult(new ServerResponse<ClientFullInfoForClients>(Utils.StatusCode.InvalidRole, null));
            }

            return new JsonResult(await _clientAccountService.GetProfileInfo(clientIdentityUser.Id));
            
            
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
        [Authorize(Roles ="Client")]
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


        [Authorize(Roles = "Client")]
        [HttpPut("SetAsMainImage")]
        public async Task<ActionResult<ServerResponse>> SetAsMainImage([FromBody]int imageId)
        {
            return new JsonResult(await _clientAccountService.SetAsMainImage(imageId));
        }


        [Authorize(Roles ="Client")]
        [HttpPost("UploadImages")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientImageInfo>>>> UploadImages([FromBody]ICollection<string> imagesString)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientAccountService.UploadImages(identityUser.Id, imagesString));
        }


        [Authorize(Roles = "Client")]
        [HttpDelete("DeleteImage/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteImage(int id)
        {
            return new JsonResult(await _clientAccountService.DeleteImage(id));
        }


        // ====================================================================================================

























    }
}