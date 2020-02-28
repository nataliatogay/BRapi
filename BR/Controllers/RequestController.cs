﻿using System;
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
        public async Task<ActionResult<ServerResponse<IEnumerable<RequestInfoResponse>>>> Get()
        {
            return new JsonResult(Response((await _clientRequestService.GetAllClientRequests()).ToList()));
        }

        [HttpGet("count")]
        public async Task<ActionResult<ServerResponse<int>>> GetCount()
        {
            return new JsonResult(Response(await _clientRequestService.ClientRequestCount()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<ClientRequest>>> Get(int id)
        {
            return new JsonResult(Response(await _clientRequestService.GetClientRequest(id)));
        }

        
    }
}