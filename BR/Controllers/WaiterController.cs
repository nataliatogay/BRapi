using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
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

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WaiterController : ControllerBase
    {
        private readonly IWaiterService _waiterService;
        private readonly UserManager<IdentityUser> _userManager;

        private readonly ISMSConfiguration _smsConfiguration;

        public WaiterController(IWaiterService waiterService, 
            UserManager<IdentityUser> userManager,
            ISMSConfiguration smsConfiguration)
        {
            _waiterService = waiterService;
            _userManager = userManager;
            _smsConfiguration = smsConfiguration;
        }

        [HttpGet("")]
        public async Task<ActionResult<ICollection<Waiter>>> Get()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if(identityUser is null)
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

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]NewWaiterRequest newWaiter)
        {
            var identityUserClient = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUserClient is null)
            {
                return null;
            }
            var password = _waiterService.GeneratePassword();
            string login = null;
            IdentityResult res = null;
            do
            {
                login = _waiterService.GenerateLogin(newWaiter.LastName);
                res = await _userManager.CreateAsync(new IdentityUser() { UserName = login, PhoneNumber = newWaiter.PhoneNumber }, password);
                
            } while (!res.Succeeded);
            var identityUserWaiter = await _userManager.FindByNameAsync(login);
            // send code
            /*
            TwilioClient.Init(_smsConfiguration.AccountSid, _smsConfiguration.AuthToken);
            var body = $"Login: {login}. Password: {password}";
            var msg = MessageResource.Create(body: body,
                from: new Twilio.Types.PhoneNumber(_smsConfiguration.PhoneNumber),
                to: new Twilio.Types.PhoneNumber(newWaiter.PhoneNumber));
            return new JsonResult(msg.Sid);
            */

            return new JsonResult(await _waiterService.AddNewWaiter(newWaiter, identityUserWaiter.Id, identityUserClient.Id));
        }
    }
}