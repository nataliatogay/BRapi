using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Privileges;
using BR.DTO.Waiters;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Twilio;
using Twilio.Rest.Api.V2010.Account;


// add update token, log out etc. 
namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WaiterController : ResponseController
    {
        private readonly IWaiterService _waiterService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        private readonly ISMSConfiguration _smsConfiguration;

        public WaiterController(IWaiterService waiterService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ISMSConfiguration smsConfiguration)
        {
            _waiterService = waiterService;
            _userManager = userManager;
            _roleManager = roleManager;
            _smsConfiguration = smsConfiguration;
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("ForOwners/{clientId}")]
        public async Task<ActionResult<ServerResponse<ICollection<WaiterInfo>>>> GetAllWaitersForOwner(int clientId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<WaiterInfo>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _waiterService.GetAllWaitersForOwner(clientId, ownerIdentityUser.Id));
        }



        [Authorize(Roles = "Client")]
        [HttpGet("ForClient")]
        public async Task<ActionResult<ServerResponse<ICollection<WaiterInfo>>>> GetAllWaitersForClient()
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<WaiterInfo>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _waiterService.GetAllWaitersForClient(clientIdentityUser.Id));
        }




        [Authorize(Roles = "Owner")]
        [HttpPost("NewByOwner")]
        public async Task<ActionResult<ServerResponse<WaiterInfo>>> AddNewWaiterByOwner([FromBody]NewWaiterByOwnerRequest waiterRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<WaiterInfo>(Utils.StatusCode.UserNotFound, null));
            }
            var password = _waiterService.GeneratePassword();
            string login = null;
            IdentityResult res = null;
            do
            {
                login = _waiterService.GenerateLogin(waiterRequest.LastName);
                res = await _userManager.CreateAsync(new IdentityUser() { UserName = login, PhoneNumber = waiterRequest.PhoneNumber }, password);

            } while (!res.Succeeded);

            var waiterIdentityUser = await _userManager.FindByNameAsync(login);
            var role = await _roleManager.FindByNameAsync("Waiter");
            if (role != null)
            {
                var resp = await _userManager.AddToRoleAsync(waiterIdentityUser, "Waiter");
                if (!resp.Succeeded)
                {
                    return new JsonResult(new ServerResponse<WaiterInfo>(Utils.StatusCode.Error, null));
                }
            }

            // send code
            /*
            TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);
            var body = $"Login: {login}. Password: {password}";
            var msg = MessageResource.Create(body: body,
                from: new Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                to: new Twilio.Types.PhoneNumber(newWaiter.PhoneNumber));
            return new JsonResult(msg.Sid);
            */

            return new JsonResult(await _waiterService.AddNewWaiterByOwner(waiterRequest, waiterIdentityUser.Id, ownerIdentityUser.Id));

        }


        [Authorize(Roles = "Client")]
        [HttpPost("NewByClient")]
        public async Task<ActionResult<ServerResponse<WaiterInfo>>> AddNewWaiterByClient([FromBody]NewWaiterByClientRequest waiterRequest)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<WaiterInfo>(Utils.StatusCode.UserNotFound, null));
            }
            var password = _waiterService.GeneratePassword();
            string login = null;
            IdentityResult res = null;
            do
            {
                login = _waiterService.GenerateLogin(waiterRequest.LastName);
                res = await _userManager.CreateAsync(new IdentityUser() { UserName = login, PhoneNumber = waiterRequest.PhoneNumber }, password);

            } while (!res.Succeeded);

            var waiterIdentityUser = await _userManager.FindByNameAsync(login);
            var role = await _roleManager.FindByNameAsync("Waiter");
            if (role != null)
            {
                var resp = await _userManager.AddToRoleAsync(waiterIdentityUser, "Waiter");
                if (!resp.Succeeded)
                {
                    return new JsonResult(new ServerResponse<WaiterInfo>(Utils.StatusCode.Error, null));
                }
            }

            // send code
            /*
            TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);
            var body = $"Login: {login}. Password: {password}";
            var msg = MessageResource.Create(body: body,
                from: new Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                to: new Twilio.Types.PhoneNumber(newWaiter.PhoneNumber));
            return new JsonResult(msg.Sid);
            */

            return new JsonResult(await _waiterService.AddNewWaiterByClient(waiterRequest, waiterIdentityUser.Id, clientIdentityUser.Id));
        }



        [Authorize(Roles = "Owner")]
        [HttpPut("UpdateByOwner")]
        public async Task<ActionResult<ServerResponse<WaiterInfo>>> UpdateWaiterByOwner([FromBody]UpdateWaiterRequest updateRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<WaiterInfo>(Utils.StatusCode.UserNotFound, null));
            }
            return new JsonResult(await _waiterService.UpdateWaiterByOwner(updateRequest, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Client")]
        [HttpPut("UpdateByClient")]
        public async Task<ActionResult<ServerResponse<WaiterInfo>>> UpdateWaiterByClient([FromBody]UpdateWaiterRequest updateRequest)
        {
            var clientIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (clientIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<WaiterInfo>(Utils.StatusCode.UserNotFound, null));
            }
            return new JsonResult(await _waiterService.UpdateWaiterByClient(updateRequest, clientIdentityUser.Id));
        }


        // =====================================================================================================================




        [HttpGet("")]
        public async Task<ActionResult<ICollection<Waiter>>> Get()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return null;
            }

            return new JsonResult((await _waiterService.GetAllWaiters(identityUser.Id)).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Waiter>> Get(int id)
        {
            return new JsonResult(await _waiterService.GetWaiter(id));
        }





        [HttpPost("AssignPrivilege")]
        public async Task<ActionResult<ServerResponse>> AssignPrivilege([FromBody]AssignPrivilegeRequest assignmentRequest)
        {
            return new JsonResult(await _waiterService.AssignPrivilege(assignmentRequest));
        }
    }
}