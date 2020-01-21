using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ControllerBase
    {
        private readonly IClientRequestService _clientRequestService;

        public RequestController(IClientRequestService clientRequestService)
        {
            _clientRequestService = clientRequestService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]NewRequestRequest newClientRequest)
        {
            //ClientRequest clientRequest = new ClientRequest()
            //{
            //    RegisteredDate = DateTime.Now,
            //    JsonInfo = JsonConvert.SerializeObject(newClientRequest)
            //};
            //await _clientRequestService.AddNewClientRequest(clientRequest);

            await _clientRequestService.AddNewClientRequest(newClientRequest);
            return Ok();
            //return new JsonResult((await _clientRequestService.GetAllClientRequests()).ToList());
        }

        [HttpGet("")]
        public async Task<ActionResult<IEnumerable<ClientRequest>>> Get()
        {
            return new JsonResult((await _clientRequestService.GetAllClientRequests()).ToList());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ClientRequest>> Get(int id)
        {
            return new JsonResult(await _clientRequestService.GetClientRequest(id));
        }

        
    }
}