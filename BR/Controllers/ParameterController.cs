using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Parameters;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //  [Authorize]
    public class ParameterController : ResponseController
    {
        private readonly IParameterService _parameterService;

        public ParameterController(IParameterService parameterService)
        {
            _parameterService = parameterService;
        }

        [AllowAnonymous]

        [HttpGet]
        public async Task<ActionResult<ClientParametersResponse>> Get()
        {
            var paymentTypes = await _parameterService.GetAllPaymentTypes();
            var cuisines = await _parameterService.GetAllCuisines();
            var clientTypes = await _parameterService.GetAllClientTypes();
            var mealTypes = await _parameterService.GetAllMealType();
            Dictionary<int, string> paymentTypesDict = new Dictionary<int, string>();
            Dictionary<int, string> cuisinesDict = new Dictionary<int, string>();
            Dictionary<int, string> clientTypesDict = new Dictionary<int, string>();
            Dictionary<int, string> mealTypesDict = new Dictionary<int, string>();

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
            foreach (var item in mealTypes)
            {
                mealTypesDict.Add(item.Id, item.Title);
            }

            return new JsonResult(new ClientParametersResponse()
            {
                ClientTypes = clientTypesDict,
                PaymentTypes = paymentTypesDict,
                Cuisines = cuisinesDict,
                MealTypes = mealTypesDict
            });
        }

        [HttpGet("info")]
        public async Task<ActionResult<ServerResponse<ParametersInfoResponse>>> GetInfo()
        {
            var paymentTypes = await _parameterService.GetAllPaymentTypes();
            var cuisines = await _parameterService.GetAllCuisines();
            var clientTypes = await _parameterService.GetAllClientTypes();
            var mealTypes = await _parameterService.GetAllMealType();
            return new JsonResult(Response(new ParametersInfoResponse()
            {
                ClientTypes = clientTypes,
                Cuisines = cuisines,
                MealTypes = mealTypes,
                PaymentTypes = paymentTypes
            }));
        }


        [HttpGet("cuisine")]
        public async Task<ActionResult<IEnumerable<Cuisine>>> GetCuisines()
        {
            return new JsonResult(await _parameterService.GetAllCuisines());
        }

        [HttpPost("cuisine")]
        public async Task<ActionResult<Cuisine>> AddCuisine([FromBody]ICollection<string> cuisineTitle)
        {
            await _parameterService.AddCuisine(cuisineTitle);
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }

        [HttpPut("cuisine")]
        public async Task<ActionResult<Cuisine>> UpdateCuisine([FromBody]ICollection<Cuisine> cuisines)
        {
           
            await _parameterService.UpdateCuisine(cuisines);
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }

        [HttpDelete("cuisine/{id}")]
        public async Task<IActionResult> DeleteCuisine(int id)
        {
            if (await _parameterService.DeleteCuisine(id))
            {
                return new JsonResult(Response(Utils.StatusCode.Ok));
            }
            return new JsonResult(Response(Utils.StatusCode.Error));
        }

        [HttpGet("clientType")]
        public async Task<ActionResult<IEnumerable<ClientType>>> GetClientTypes()
        {
            return new JsonResult(await _parameterService.GetAllClientTypes());
        }

        [HttpPost("clientType")]
        public async Task<ActionResult<Cuisine>> AddClientType([FromBody]ICollection<string> clientTypeTitles)
        {
            await _parameterService.AddClientType(clientTypeTitles);
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }

        [HttpPut("clientType")]
        public async Task<ActionResult<ClientType>> UpdateClientType([FromBody]ICollection<ClientType> clientTypes)
        {
            await _parameterService.UpdateClientType(clientTypes);
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }

        [HttpDelete("clientType/{id}")]
        public async Task<ActionResult<IEnumerable<ClientType>>> DeleteClientType(int id)
        {
            if (await _parameterService.DeleteClientType(id))
            {
                return new JsonResult(Response(Utils.StatusCode.Ok));
            }
            return new JsonResult(Response(Utils.StatusCode.Error));
        }


        [HttpGet("paymentType")]
        public async Task<ActionResult<IEnumerable<Cuisine>>> GetPaymentTypes()
        {
            return new JsonResult(await _parameterService.GetAllPaymentTypes());
        }

        [HttpPost("paymentType")]
        public async Task<ActionResult<Cuisine>> AddPaymentType([FromBody]ICollection<string> paymentTypeTitle)
        {
            await _parameterService.AddPaymentType(paymentTypeTitle);
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }

        [HttpPut("paymentType")]
        public async Task<ActionResult<Cuisine>> UpdatePaymentType([FromBody]ICollection<PaymentType> paymentTypes)
        {
            await _parameterService.UpdatePaymentType(paymentTypes);
            return new JsonResult(Response(Utils.StatusCode.Ok));
        }

        [HttpDelete("paymentType/{id}")]
        public async Task<IActionResult> DeletePaymentType(int id)
        {
            if (await _parameterService.DeletePaymentType(id))
            {
                return new JsonResult(Response(Utils.StatusCode.Ok));
            }
            return new JsonResult(Response(Utils.StatusCode.Error));
        }

    }
}