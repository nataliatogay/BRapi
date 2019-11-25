using System;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClientAccountService _clientAccountService;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public ClientAccountController(UserManager<IdentityUser> userManager,
            IClientAccountService clientAccountService,
            IEmailService emailService,
            IMemoryCache cache)
        {
            _userManager = userManager;
            _clientAccountService = clientAccountService;
            _emailService = emailService;
            _cache = cache;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody]LogInRequest model)
        {
            var identityUser = await _userManager.FindByNameAsync(model.Email);

            if (identityUser != null)
            {
                if (!(await _clientAccountService.ClientIsBlocked(identityUser.Id)))
                {
                    bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                    if (checkPassword)
                    {
                        // надо ли - ??
                        //if(!(await _userManager.IsEmailConfirmedAsync(identityUser))) { }
                        LogInResponse resp = await _clientAccountService.LogIn(identityUser);
                        if (resp != null)
                        {
                            return new JsonResult(resp);
                        }
                    }
                }
                else
                {
                    return new JsonResult("Client is blocked");
                }
            }

            return new JsonResult("Invalid data");
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


       // [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody]string refreshToken)
        {
            await _clientAccountService.LogOut(refreshToken);
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
                    throw ex;
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
      //  [ValidateAntiForgeryToken] -> BadRequest
        public async Task<IActionResult> ForgotPassword([FromBody]string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user == null)
            {
                return new JsonResult("Client not found");
            }

            if (await _clientAccountService.ClientIsBlocked(user.Id))
            {
                return new JsonResult("Client is blocked");
            }

            var code = await _userManager.GeneratePasswordResetTokenAsync(user);
            var callbackUrl = Url.Action("ResetPassword",
                "ClientAccount",
                new { userId = user.Id, code = code },
                protocol: HttpContext.Request.Scheme);
            try
            {
                string msgBody = $"<a href='{callbackUrl}'>link</a>";

                await _emailService.SendAsync(email, "Paasword reset", msgBody);
                return Ok();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpGet("ResetPassword")]
        [AllowAnonymous]
        public IActionResult ResetPassword(string code = null)
        {
            if(code is null)
            {
                return new JsonResult("Error");
            } else
            {
                return new JsonResult(code);
            }
        }

        [HttpPost("ResetPassword")]
        [AllowAnonymous]
     //   [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordRequest model)
        {
            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null)
            {
                return new JsonResult("Error");
            }
            var result = await _userManager.ResetPasswordAsync(user, model.Code, model.Password);
            if (result.Succeeded)
            {
                return Ok();
            }
            return new JsonResult("Error");
        }


    }
}