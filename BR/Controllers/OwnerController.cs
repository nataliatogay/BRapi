using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO.Owners;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OwnerController : ResponseController
    {
        private readonly IOwnerService _ownerService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationService _authenticationService;


        public OwnerController(IOwnerService ownerService,
            UserManager<IdentityUser> userManager,
            IEmailService emailService,
            RoleManager<IdentityRole> roleManager,
            IAuthenticationService authenticationService)
        {
            _ownerService = ownerService;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _authenticationService = authenticationService;

        }


        [Authorize(Roles = "Admin")]
        [HttpPost("")]
        public async Task<ActionResult<ServerResponse>> Post([FromBody]NewOwnerRequest newOwnerRequest)
        {
            string password = _authenticationService.GeneratePassword();
            if (await _userManager.FindByNameAsync(newOwnerRequest.Email) is null)
            {
                IdentityUser identityUser = new IdentityUser()
                {
                    Email = newOwnerRequest.Email,
                    UserName = newOwnerRequest.Email
                };

                IdentityResult res = await _userManager.CreateAsync(identityUser, password);
                if (res.Succeeded)
                {
                    identityUser = await _userManager.FindByNameAsync(newOwnerRequest.Email);
                    await _userManager.SetPhoneNumberAsync(identityUser, newOwnerRequest.OwnerNumber);
                    var role = await _roleManager.FindByNameAsync("Owner");
                    if (role != null)
                    {
                        var resp = await _userManager.AddToRoleAsync(identityUser, "Owner");
                        if (!resp.Succeeded)
                        {
                            return new JsonResult(Response(Utils.StatusCode.Error));
                        }
                    }
                    else
                    {
                        return new JsonResult(Response(Utils.StatusCode.RoleNotFound));
                    }
                    ServerResponse addResponse;
                    try
                    {
                        addResponse = await _ownerService.AddNewOwner(newOwnerRequest, identityUser.Id);
                    }
                    catch
                    {
                        return new JsonResult(Response(Utils.StatusCode.Error));
                    }
                    if (addResponse.StatusCode == Utils.StatusCode.Ok)
                    {
                        try
                        {
                            string msgBody = $"Login: {identityUser.Email}\nPassword: {password}";

                            await _emailService.SendAsync(identityUser.Email, "Registration info ", msgBody);
                            return new JsonResult(Response(Utils.StatusCode.Ok));
                        }
                        catch
                        {
                            return new JsonResult(Response(Utils.StatusCode.SendingMailError));
                        }
                    }
                    else
                    {
                        return new JsonResult(addResponse);
                    }

                }
                else
                {
                    return new JsonResult(Response(Utils.StatusCode.Error));
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.EmailUsed));
            }


        }


        [Authorize(Roles = "Owner")]
        [HttpGet("ForOwners")]
        public async Task<ActionResult<ServerResponse<OwnerInfoForOwners>>> GetOwnerInfoForOwners()
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<OwnerInfoForOwners>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _ownerService.GetOwnerInfoForOwners(ownerIdentityUser.Id));
        }
    }
}