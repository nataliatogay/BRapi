using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        

        public ClientController(IClientService clientService,
            UserManager<IdentityUser> userManager)
        {
            _clientService = clientService;
            _userManager = userManager;
            
        }

        
        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<Client>>> GetClients()
        {
            //return (await _clientService.GetAllClients()).ToList();
            return new JsonResult((await _clientService.GetAllClients()).ToList());
        }

        [HttpGet("{id}")] 
        public async Task<IActionResult> GetClient(int id)
        {
            return new JsonResult(await _clientService.GetClient(id));
        }

        /*

        [HttpPost("")]
        public async Task<IActionResult> NewClient([FromBody]Client client)
        {
            // string password = Guid.NewGuid().ToString();
            string password = _clientService.GeneratePassword();
            IdentityUser user = new IdentityUser()
            {
                Email = client.Email,
                UserName = client.Email 
            };

            IdentityResult res = await _userManager.CreateAsync(user, password);

            user = await _userManager.FindByNameAsync(client.Email);
            client.IdentityId = user.Id;

            client.Password = password;

            await _clientService.AddNewClient(client);
            return new JsonResult((await _clientService.GetAllClients()).ToList()) { StatusCode = 201 };
        }
        */

        [HttpPut("")]
        public async Task<IActionResult> UpdateClient([FromBody]Client client)
        {
            client = await _clientService.UpdateClient(client);
            return new JsonResult(await _clientService.GetClient(client.Id));
        }

        [HttpDelete("{id}")] 
        public async Task<IActionResult> DeleteClient(int id) 
        {
            await _clientService.DeleteClient(id);
            return new JsonResult((await _clientService.GetAllClients()).ToList());
        }

        [HttpGet("Requests")]
        public async Task<ActionResult<IEnumerable<ToBeClient>>> ToBeClients()
        {
            return new JsonResult((await _clientService.GetAllToBeClients()).ToList());
        }

        [HttpGet("Requests/{id}")]
        public async Task<ActionResult<ToBeClient>> GetToBeClient(int id)
        {
            return new JsonResult(await _clientService.GetToBeClient(id));
        }



        [HttpPost("NewRequest")]
        public async Task<IActionResult> AddNewToBeClient([FromBody]Client client)
        {
            ToBeClient toBeClient = new ToBeClient()
            {
                RegisteredDate = DateTime.Now,
                JsonInfo = JsonConvert.SerializeObject(client)
            };
            await _clientService.AddNewToBeClient(toBeClient);
            return new JsonResult((await _clientService.GetAllToBeClients()).ToList());
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
