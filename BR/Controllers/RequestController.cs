using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Http;
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
            ClientRequest clientRequest = new ClientRequest()
            {
                RegisteredDate = DateTime.Now,
                JsonInfo = JsonConvert.SerializeObject(newClientRequest)
            };
            await _clientRequestService.AddNewClientRequest(clientRequest);
            return new JsonResult((await _clientRequestService.GetAllClientRequests()).ToList());
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

        [HttpGet("params")]
        public async Task<ActionResult<ClientParametersRequest>> GetParams()
        {
            var paymentTypes = await _clientRequestService.GetAllPaymentTypes();
            var cuisines = await _clientRequestService.GetAllCuisines();
            var clientTypes = await _clientRequestService.GetAllClientTypes();
            Dictionary<int, string> paymentTypesDict = new Dictionary<int, string>();
            Dictionary<int, string> cuisinesDict = new Dictionary<int, string>();
            Dictionary<int, string> clientTypesDict = new Dictionary<int, string>();

            foreach (var item in paymentTypes)
            {
                paymentTypesDict.Add(item.Id, item.Title);
            }
            foreach (var item in cuisines)
            {
                cuisinesDict.Add(item.Id, item.Title);
            }
            foreach (var item in clientTypes)
            {
                clientTypesDict.Add(item.Id, item.Title);
            }
            return new JsonResult(new ClientParametersRequest()
            {
                ClientTypes = clientTypesDict,
                PaymentTypes = paymentTypesDict,
                Cuisines = cuisinesDict
            });

            //return new JsonResult(new ClientParametersRequest()
            //{
            //    ClientTypes = clientTypes,
            //    Cuisines = cuisines,
            //    PaymentTypes = paymentTypes
            //});
        }
    }
}