using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class WaiterController : ControllerBase
    {
        private readonly IWaiterService _waiterService;
        private readonly UserManager<IdentityUser> _userManager;

        public WaiterController(IWaiterService waiterService, 
            UserManager<IdentityUser> userManager)
        {
            _waiterService = waiterService;
            _userManager = userManager;
        }
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Waiter>>> Get()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if(identityUser is null)
            {
                return null;
            }

            return new JsonResult((await _waiterService.GetAllWaiters(identityUser.Id)).ToList());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
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
                res = await _userManager.CreateAsync(new IdentityUser() { UserName = login }, password);
                
            } while (!res.Succeeded);
            var identityUserWaiter = await _userManager.FindByNameAsync(login);
            return new JsonResult(await _waiterService.AddNewWaiter(newWaiter, identityUserWaiter.Id, identityUserClient.Id));
        }
    }
}