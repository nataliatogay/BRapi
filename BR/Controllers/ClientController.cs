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

namespace BR.Controllers
{
    // [Authorize]
   // [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ResponseController
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
        public async Task<ActionResult<ServerResponse<IEnumerable<ClientInfoResponse>>>> Get()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            var res = await _clientService.GetAllClients(role);
            return new JsonResult(Response(res));
        }

        [HttpGet("mealtype")]
        public async Task<ActionResult<ServerResponse<IEnumerable<ClientInfoResponse>>>> Get(string mealType)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;
            return new JsonResult(Response(await _clientService.GetClientsByMeal(mealType, role)));
        }


        [HttpGet("search")]
        public async Task<ActionResult<ServerResponse<IEnumerable<ClientInfoResponse>>>> Search(string name)
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            return new JsonResult(Response(await _clientService.GetClientsByName(name, role)));
        }


        //?
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<ClientInfoResponse>>> Get(int id)
        {
            string role = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimsIdentity.DefaultRoleClaimType)?.Value;

            return new JsonResult(Response(await _clientService.GetClient(id, role)));
        }


        [HttpGet("schema/{id}")]
        public async Task<ActionResult<ServerResponse<ClientHallsInfoResponse>>> ClientSchema(int id)
        {
            return new JsonResult(Response(await _clientService.GetClientHalls(id)));
        }




        [HttpPost("")]
        public async Task<ActionResult<ServerResponse>> Post([FromBody]NewClientRequest newClient)
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
                    return new JsonResult(Response(Controllers.StatusCode.Error));
                }
            }

            return new JsonResult(Response(await _clientService.GetAllClients("Admin")));






           // return new JsonResult((await _clientService.GetAllClients()).ToList()) { StatusCode = 201 };
        }


        [HttpPut("")]
        public async Task<ActionResult<ServerResponse<Client>>> Update([FromBody]Client client)
        {
             return new JsonResult(Response(await _clientService.UpdateClient(client)));
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