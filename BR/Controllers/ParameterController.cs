using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
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
        public async Task<ActionResult<Cuisine>> AddCuisine([FromBody]string cuisineTitle)
        {
            return new JsonResult(await _parameterService.AddCuisine(cuisineTitle));
        }

        [HttpPut("cuisine")]
        public async Task<ActionResult<Cuisine>> UpdateCuisine([FromBody]Cuisine cuisine)
        {
            return new JsonResult(await _parameterService.UpdateCuisine(cuisine));
        }

        [HttpDelete("cuisine/{id}")]
        public async Task<IActionResult> DeleteCuisine(int id)
        {
            if (await _parameterService.DeleteCuisine(id))
            {
                return new JsonResult((await _parameterService.GetAllCuisines()).ToList());
            }
            return new JsonResult("Cannot delete");
        }

        [HttpGet("clientType")]
        public async Task<ActionResult<IEnumerable<ClientType>>> GetClientTypes()
        {
            return new JsonResult(await _parameterService.GetAllClientTypes());
        }

        [HttpPost("clientType")]
        public async Task<ActionResult<Cuisine>> AddClientType([FromBody]string clientTypeTitle)
        {
            return new JsonResult(await _parameterService.AddClientType(clientTypeTitle));
        }

        [HttpPut("clientType")]
        public async Task<ActionResult<ClientType>> UpdateClientType([FromBody]ClientType clientType)
        {
            return new JsonResult(await _parameterService.UpdateClientType(clientType));
        }

        [HttpDelete("clientType/{id}")]
        public async Task<ActionResult<IEnumerable<ClientType>>> DeleteClientType(int id)
        {
            if (await _parameterService.DeleteClientType(id))
            {
                return new JsonResult((await _parameterService.GetAllClientTypes()).ToList());
            }
            return new JsonResult("Cannot delete");
        }


        [HttpGet("paymentType")]
        public async Task<ActionResult<IEnumerable<Cuisine>>> GetPaymentTypes()
        {
            return new JsonResult(await _parameterService.GetAllPaymentTypes());
        }

        [HttpPost("paymentType")]
        public async Task<ActionResult<Cuisine>> AddPaymentType([FromBody]string paymentTypeTitle)
        {
            return new JsonResult(await _parameterService.AddPaymentType(paymentTypeTitle));
        }

        [HttpPut("paymentType")]
        public async Task<ActionResult<Cuisine>> UpdatePaymentType([FromBody]PaymentType paymentType)
        {
            return new JsonResult(await _parameterService.UpdatePaymentType(paymentType));
        }

        [HttpDelete("paymentType/{id}")]
        public async Task<IActionResult> DeletePaymentType(int id)
        {
            if (await _parameterService.DeletePaymentType(id))
            {
                return new JsonResult((await _parameterService.GetAllPaymentTypes()).ToList());
            }
            return new JsonResult("Cannot delete");
        }

    }
}