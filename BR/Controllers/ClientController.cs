using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BR.Features.Admin
{
    // [Authorize]
   // [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;


        public ClientController(IClientService clientService,
            UserManager<IdentityUser> userManager,
            IEmailService emailService)
        {
            _clientService = clientService;
            _userManager = userManager;
            _emailService = emailService;

        }

        // for users and clients
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ClientInfoResponse>>> Get()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            return new JsonResult(await _clientService.GetAllClients(role));
          
        }

        [HttpGet("mealtype")]
        public async Task<ActionResult<IEnumerable<ClientInfoResponse>>> Get(string mealType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            return new JsonResult(await _clientService.GetClientsByMeal(mealType, role));
        }


        [HttpGet("search")]
        public async Task<ActionResult<IEnumerable<ClientInfoResponse>>> Search(string name)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            return new JsonResult(await _clientService.GetClientsByName(name, role));
        }


        //?
        [HttpGet("{id}")]
        public async Task<ActionResult<ClientInfoResponse>> Get(int id)
        {
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            return new JsonResult(await _clientService.GetClient(id, role));
        }


        [HttpGet("schema/{id}")]
        public async Task<ActionResult<ClientInfoResponse>> ClientSchema(int id)
        {
            return new JsonResult(await _clientService.GetClientHalls(id));
        }




        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]NewClientRequest newClient)
        {
             string password = _clientService.GeneratePassword();
            IdentityUser identityUser = new IdentityUser()
            {
                Email = newClient.Email,
                UserName = newClient.Email
            };

            IdentityResult res = await _userManager.CreateAsync(identityUser, password);
            if (res.Succeeded)
            {
                identityUser = await _userManager.FindByNameAsync(newClient.Email);
                await _clientService.AddNewClient(newClient, identityUser.Id);
                try
                {
                    string msgBody = $"Login: {identityUser.Email}\nPassword: {password}";

                    await _emailService.SendAsync(identityUser.Email, "Registration info", msgBody);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }

            return new JsonResult(await _clientService.GetAllClients("Admin"));






           // return new JsonResult((await _clientService.GetAllClients()).ToList()) { StatusCode = 201 };
        }


        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody]Client client)
        {
            client = await _clientService.UpdateClient(client);
            return Ok();
            //return new JsonResult(await _clientService.GetClient(client.Id));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _clientService.DeleteClient(id);
            return Ok();
          //  return new JsonResult((await _clientService.GetAllClients()).ToList());
        }


        

    }
}