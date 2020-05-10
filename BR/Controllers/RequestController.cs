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
        private readonly IOwnerRequestService _clientRequestService;

        public RequestController(IOwnerRequestService clientRequestService)
        {
            _clientRequestService = clientRequestService;
        }

        [HttpPost("")]
        public async Task<ActionResult<ServerResponse>> Post([FromBody]NewOwnerRequestRequest newClientRequest)
        {
            return new JsonResult(await _clientRequestService.AddNewOwnerRequest(newClientRequest));
        }



        [HttpGet("")]
        public async Task<ActionResult<ServerResponse<IEnumerable<OwnerRequestInfo>>>> Get()
        {
            return new JsonResult(await _clientRequestService.GetAllOwnerRequests());
        }



        //[HttpGet("UndoneCount")]
        //public async Task<ActionResult<ServerResponse<int>>> GetUndoneCount()
        //{
        //    return new JsonResult(await _clientRequestService.UndoneClientRequestCount());
        //}

        [HttpGet("Count")]
        public async Task<ActionResult<ServerResponse<int>>> GetCount()
        {
            return new JsonResult(await _clientRequestService.OwnerRequestCount());
        }


        
        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<OwnerRequest>>> Get(int id)
        {
            return new JsonResult(await _clientRequestService.GetOwnerRequest(id));
        }



        [HttpPut("Decline")]
        public async Task<ActionResult<ServerResponse<OwnerRequest>>> DeclineRequest([FromBody]int id)
        {
            return new JsonResult(await _clientRequestService.DeclineOwnerRequest(id));
        }


    }
}