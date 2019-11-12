using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientAccountController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IClientAccountService _clientAccountService;

        public ClientAccountController(UserManager<IdentityUser> userManager,
            IClientAccountService clientAccountService)
        {
            _userManager = userManager;
            _clientAccountService = clientAccountService;
        }

        [HttpPost("LogIn")]
        public async Task<IActionResult> LogIn([FromBody]LogInRequest model)
        {
            var identityUser = await _userManager.FindByNameAsync(model.Email);
            if (identityUser != null)
            {
                bool checkPassword = await _userManager.CheckPasswordAsync(identityUser, model.Password);
                if (checkPassword)
                {
                    LogInResponse resp = await _clientAccountService.LogIn(identityUser);
                    if (resp != null)
                    {
                        return new JsonResult(resp);
                    }
                }
            }

            return BadRequest();
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
        [HttpGet("LogOut")]
        public async Task<IActionResult> LogOut(string refreshToken)
        {

            // string id = HttpContext.User.Claims.FirstOrDefault(c => c.Type == "id")?.Value;

            //if (id == null)
            //{
            //    return new JsonResult("no admin");
            //}
            await _clientAccountService.LogOut(refreshToken);

            return Ok();
            //return new JsonResult(_accountService.LogOut(Int32.Parse(id)));
            //return Ok();
        }
    }
}