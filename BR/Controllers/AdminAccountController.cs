using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Admin;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json.Linq;

namespace BR.Controllers
{




    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ResponseController
    {
        private readonly IAdminAccountService _adminAccountService;
        private readonly IAuthenticationService _authenticationService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public AdminAccountController(IAdminAccountService adminAccountService,
            IAuthenticationService authenticationService,
            UserManager<IdentityUser> userManager,
            IEmailService emailService,
            RoleManager<IdentityRole> roleManager,
            IMemoryCache cache)
        {
            _adminAccountService = adminAccountService;
            _authenticationService = authenticationService;
            _userManager = userManager;
            _emailService = emailService;
            _cache = cache;
            _roleManager = roleManager; _roleManager = roleManager;
        }



        //[HttpPost("Login")]
        //public async Task<ActionResult<Response<LogInResponse>>> LogInDemo([FromBody]LogInRequest model)
        //{
        //    return Response(new LogInResponse());
        //}


        [HttpPost("Login")]
        public async Task<ActionResult<ServerResponse<LogInResponse>>> LogIn([FromBody]LogInRequest model)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(model.Email);
            if (identityUser != null)
            {
                var roles = await _userManager.GetRolesAsync(identityUser);
                if (!roles.Contains("Admin"))
                {
                    return new JsonResult(Response(Utils.StatusCode.InvalidRole));
                }
                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                bool emailConfirm = await _userManager.IsEmailConfirmedAsync(identityUser);
                if (!emailConfirm)
                {
                    return new JsonResult(Response(Utils.StatusCode.EmailNotConfirmed));
                }
                if (checkPassword)
                {

                    var resp = await _adminAccountService.LogIn(identityUser.UserName, model.NotificationTag);

                    return new JsonResult(resp);
                }
                else
                {
                    return new JsonResult(new ServerResponse<LogInResponse>(Utils.StatusCode.IncorrectLoginOrPassword, null));
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));

            }

        }

        [HttpPost("Token")] //api/account/token
        public async Task<ActionResult<ServerResponse<LogInResponse>>> UpdateToken([FromBody]string refreshToken)
        {
            return new JsonResult(await _adminAccountService.UpdateToken(refreshToken));
        }

        [Authorize]
        [HttpGet("Info")]
        public async Task<ActionResult<ServerResponse<AdminInfo>>> GetInfo()
        { // claim based policy               
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(await _adminAccountService.GetAdminInfo(identityUser.Id));
        }





        [HttpPost("AddRole")]
        public async Task<IActionResult> AddRole([FromBody]string role)
        {
            var res = await _roleManager.CreateAsync(new IdentityRole(role));
            return new JsonResult(res);
        }

        [HttpGet("AddUserRole")]
        public async Task<IActionResult> AddRoleToIdentityUser()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var res = await _userManager.AddToRoleAsync(user, "Admin");
            //var res = await _roleManager.CreateAsync(new IdentityRole(role));
            return new JsonResult(res);
        }


        [HttpPost("Register")]
        public async Task<IActionResult> AdminRegister([FromBody]string adminEmail)
        {
            var identityUser = await _userManager.FindByEmailAsync(adminEmail);
            if(identityUser != null)
            {
                return new JsonResult("Mail used");
            }

            identityUser = new IdentityUser()
            {
                Email = adminEmail,
                UserName = adminEmail
            };
            IdentityResult res = await _userManager.CreateAsync(identityUser, "admin");
            if (!res.Succeeded)
            {
                StringBuilder errors = new StringBuilder();
                foreach (var error in res.Errors)
                {
                    errors.Append(error);
                }
                return new JsonResult(errors.ToString());
            }
            identityUser = await _userManager.FindByNameAsync(adminEmail);
            var role = await _roleManager.FindByNameAsync("Admin");
            if (role != null)
            {
                var resp = await _userManager.AddToRoleAsync(identityUser, "Admin");
                if (!resp.Succeeded)
                {
                    return new JsonResult(Response(Utils.StatusCode.Error));
                }
            }

            _cache.Set(identityUser.Id, adminEmail, TimeSpan.FromMinutes(3));

            var emailConfirmationCode = await _userManager.GenerateEmailConfirmationTokenAsync(identityUser);
            var callbackUrl = Url.Action(
                        "ConfirmEmail",
                        "AdminAccount",
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
                _cache.Remove(identityUser.Id);
                throw ex;
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



        [Authorize]
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
                catch (Exception ex)
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


        [Authorize]
        [HttpPost("LogOut")]
        //[AllowAnonymous]
        public async Task<ActionResult<ServerResponse>> LogOut([FromBody]string notificationTag)
        {
            try
            {
                await _adminAccountService.LogOut(notificationTag);
            }
            catch { }
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }




        [HttpPost("ForgotPassword")]
        [AllowAnonymous]
        public async Task<ActionResult<ServerResponse>> ForgotPassword([FromBody]string email)
        {

            if (!_cache.TryGetValue(email, out _))
            {
                var identityUser = await _userManager.FindByNameAsync(email);
                if (identityUser == null)
                {
                    return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                }
                var roles = await _userManager.GetRolesAsync(identityUser);
                if (!roles.Contains("Admin"))
                {
                    return new JsonResult(Response(Utils.StatusCode.InvalidRole));
                }
                var code = _authenticationService.GenerateCode();
                _cache.Set(email, code, TimeSpan.FromMinutes(2));

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
    }
}