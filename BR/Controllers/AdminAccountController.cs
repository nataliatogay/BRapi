using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminAccountController : ControllerBase
    {
        private readonly IAdminAccountService _adminAccountService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        private readonly IMemoryCache _cache;

        public AdminAccountController(IAdminAccountService adminAccountService,
            UserManager<IdentityUser> userManager,
            IEmailService emailService,
            RoleManager<IdentityRole> roleManager,
            IMemoryCache cache)
        {
            _adminAccountService = adminAccountService;
            _userManager = userManager;
            _emailService = emailService;
            _cache = cache;
            _roleManager = roleManager;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> LogIn([FromBody]LogInRequest model)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(model.Email);
            if (identityUser != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                bool emailConfirm = await _userManager.IsEmailConfirmedAsync(identityUser);
                if (!emailConfirm)
                {
                    return new JsonResult("Email is not confirmed");
                }
                if (checkPassword)
                {
                    LogInResponse resp = await _adminAccountService.LogIn(identityUser.UserName, identityUser.Id);
                    if (resp != null)
                    {
                        return new JsonResult(resp);
                    }
                }
            }

            return new JsonResult(null);
        }

        [HttpPost("Token")] //api/account/token
        public async Task<IActionResult> UpdateToken([FromBody]string refreshToken)
        {
            LogInResponse resp = await _adminAccountService.UpdateToken(refreshToken);
            if (resp is null)
            {
                return StatusCode(401);
            }
            return new JsonResult(resp);
        }

        [Authorize]
        [HttpGet("Info")]
        public async Task<IActionResult> GetInfo()
        { // claim based policy               
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser == null)
            {
                return new JsonResult(null);
            }

            Admin adminAccount = await _adminAccountService.GetAdmin(identityUser.Id);
            if (adminAccount is null)
            {
                return new JsonResult(null);
            }
            return new JsonResult(adminAccount);
        }

        [Authorize]
        [HttpPost("LogOut")]
        public async Task<IActionResult> LogOut([FromBody]string refreshToken)
        {
            await _adminAccountService.LogOut(refreshToken);
            return Ok();
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
            if (identityUser is null || !(await _userManager.IsEmailConfirmedAsync(identityUser)))
            {
                if(identityUser == null)
                {
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
                }
                else
                {
                    string emailCache = null;
                    _cache.TryGetValue(identityUser.Id, out emailCache);
                    if(emailCache != null)
                    {
                        return new JsonResult("Link has already been sent");
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
            else
            {
                return new JsonResult("Email is already used");
            }
        }

        [Authorize]
        [HttpPost("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody]string newPassword)
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
                    await _userManager.UpdateAsync(identityUser);
                    return Ok();
                }
                else
                {
                    StringBuilder errors = new StringBuilder();
                    foreach (var error in result.Errors)
                    {
                        errors.Append(error);
                    }
                    return new JsonResult(errors.ToString());
                }
            }
            return new JsonResult("Admin not found");
        }



        [Authorize]
        [HttpPost("ChangeEmail")]
        public async Task<IActionResult> ChangeEmail([FromBody]string newEmail)
        {
            IdentityUser identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult("Admin not found");
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

    }
}