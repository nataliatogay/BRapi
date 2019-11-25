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
using Newtonsoft.Json;

namespace BR.Features.Admin
{
   // [Authorize]
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

        
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Client>>> Get()
        {
            return new JsonResult((await _clientService.GetAllClients()).ToList());
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> Get(int id)
        {
            return new JsonResult(await _clientService.GetClient(id));
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






            return new JsonResult((await _clientService.GetAllClients()).ToList()) { StatusCode = 201 };
        }
        

        [HttpPut("")]
        public async Task<IActionResult> Update([FromBody]Client client)
        {
            client = await _clientService.UpdateClient(client);
            return new JsonResult(await _clientService.GetClient(client.Id));
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> Delete(int id)  
        {
            await _clientService.DeleteClient(id);
            return new JsonResult((await _clientService.GetAllClients()).ToList());
        }

        



        
    }
}


/*
 
    {
	"Name": "Client new",
	"Address" : "Address new",
	"Latitude" : 7.7,
	"Longitude" : 8.8,
	"StartTime" : 730,
	"EndTime" : 3000,
	"IsPasking" : true,
	"IsWiFi" : false,
	"IsLiveMusic": false,
	"IsOpenSpace": false,
    "IsChildrenZone": false,
	"AdditionalInfo" : "AdditionalInfo new",
    "Email": "new_email@email.com",
    "MainImagePath": "main_image_path_new",
    "MaxReservDays": 4
}
 */
