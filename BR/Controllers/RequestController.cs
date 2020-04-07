using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Requests;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RequestController : ResponseController
    {
        private readonly IClientRequestService _clientRequestService;

        public RequestController(IClientRequestService clientRequestService)
        {
            _clientRequestService = clientRequestService;
        }

        [HttpPost("")]
        public async Task<ActionResult<ServerResponse>> Post([FromBody]NewClientRequestRequest newClientRequest)
        {
            return new JsonResult(await _clientRequestService.AddNewClientRequest(newClientRequest));
        }

        

        [HttpGet("")]
        public async Task<ActionResult<ServerResponse<IEnumerable<RequestInfoResponse>>>> Get()
        {
            return new JsonResult(await _clientRequestService.GetAllClientRequests());
        }

        [HttpGet("UndoneCount")]
        public async Task<ActionResult<ServerResponse<int>>> GetUndoneCount()
        {
            return new JsonResult(await _clientRequestService.UndoneClientRequestCount());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<ClientRequest>>> Get(int id)
        {
            return new JsonResult(await _clientRequestService.GetClientRequest(id));
        }

        
    }
}