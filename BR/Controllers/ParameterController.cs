﻿using System;
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


        [HttpGet()]
        public async Task<ActionResult<ServerResponse<ClientParametersInfo>>> GetInfo()
        {
            return new JsonResult(await _parameterService.GetClientParamenters());
        }


        [HttpGet("parmetersForUsers")]
        public async Task<ActionResult<ServerResponse<ClientParametersForUsers>>> GetInfoForUsers()
        {
            return new JsonResult(await _parameterService.GetClientParamentersForUsers());
        }


        [HttpGet("mealType")]
        public async Task<ActionResult<ServerResponse<ICollection<ParameterInfo>>>> GetMealTypes()
        {
            return new JsonResult(await _parameterService.GetAllMealTypes());
        }


        //[Authorize(Policy = "Headwaiter")]
        [HttpGet("cuisine")]
        public async Task<ActionResult<ServerResponse<ICollection<ParameterInfo>>>> GetCuisines()
        {
            return new JsonResult(await _parameterService.GetAllCuisines());
        }


        [HttpPost("cuisine")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> AddCuisine([FromBody]string title)
        {
            return new JsonResult(await _parameterService.AddCuisine(title));
        }


        [HttpPut("cuisine")]
        public async Task<ActionResult<ServerResponse>> UpdateCuisine([FromBody]ParameterInfo cuisine)
        {
            return new JsonResult(await _parameterService.UpdateCuisine(cuisine));
        }


        [HttpDelete("cuisine/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteCuisine(int id)
        {
            return new JsonResult(await _parameterService.DeleteCuisine(id));
        }


        [HttpGet("clientType")]
        public async Task<ActionResult<ServerResponse<ICollection<ParameterInfo>>>> GetClientTypes()
        {
            return new JsonResult(await _parameterService.GetAllClientTypes());
        }


        [HttpPost("clientType")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> AddClientType([FromBody]string title)
        {
            return new JsonResult(await _parameterService.AddClientType(title));
        }


        [HttpPut("clientType")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> UpdateClientType([FromBody]ParameterInfo clientType)
        {
            return new JsonResult(await _parameterService.UpdateClientType(clientType));
        }


        [HttpDelete("clientType/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteClientType(int id)
        {
            return new JsonResult(await _parameterService.DeleteClientType(id));
        }



        [HttpGet("goodFor")]
        public async Task<ActionResult<ServerResponse<ICollection<ParameterInfo>>>> GetGoodFors()
        {
            return new JsonResult(await _parameterService.GetAllGoodFors());
        }


        [HttpPost("goodFor")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> AddGoodFor([FromBody]string title)
        {
            return new JsonResult(await _parameterService.AddGoodFor(title));
        }


        [HttpPut("goodFor")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> UpdateGoodFor([FromBody]ParameterInfo goodFor)
        {
            return new JsonResult(await _parameterService.UpdateGoodFor(goodFor));
        }


        [HttpDelete("goodFor/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteGoodFor(int id)
        {
            return new JsonResult(await _parameterService.DeleteGoodFor(id));
        }


        [HttpGet("feature")]
        public async Task<ActionResult<ServerResponse<ICollection<ParameterInfo>>>> GetFeatures()
        {
            return new JsonResult(await _parameterService.GetAllFeatures());
        }


        [HttpPost("feature")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> AddFeature([FromBody]string title)
        {
            return new JsonResult(await _parameterService.AddFeature(title));
        }


        [HttpPut("feature")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> UpdateFeature([FromBody]ParameterInfo feature)
        {
            return new JsonResult(await _parameterService.UpdateFeature(feature));
        }


        [HttpDelete("feature/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteFeature(int id)
        {
            return new JsonResult(await _parameterService.DeleteFeature(id));
        }


        [HttpGet("dish")]
        public async Task<ActionResult<ServerResponse<ICollection<ParameterInfo>>>> GetDishes()
        {
            return new JsonResult(await _parameterService.GetAllDishes());
        }


        [HttpPost("dish")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> AddDish([FromBody]string title)
        {
            return new JsonResult(await _parameterService.AddDish(title));
        }


        [HttpPut("dish")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> UpdateDish([FromBody]ParameterInfo dish)
        {
            return new JsonResult(await _parameterService.UpdateDish(dish));
        }


        [HttpDelete("dish/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteDish(int id)
        {
            return new JsonResult(await _parameterService.DeleteDish(id));
        }


        [HttpGet("specialDiet")]
        public async Task<ActionResult<ServerResponse<ICollection<ParameterInfo>>>> GetSpecialDiets()
        {
            return new JsonResult(await _parameterService.GetAllSpecialDiets());
        }


        [HttpPost("specialDiet")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> AddSpecialDiet([FromBody]string title)
        {
            return new JsonResult(await _parameterService.AddSpecialDiet(title));
        }


        [HttpPut("specialDiet")]
        public async Task<ActionResult<ServerResponse<ParameterInfo>>> UpdateSpecialDiet([FromBody]ParameterInfo specialDiet)
        {
            return new JsonResult(await _parameterService.UpdateSpecialDiet(specialDiet));
        }


        [HttpDelete("specialDiet/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteSpecialDiet(int id)
        {
            return new JsonResult(await _parameterService.DeleteSpecialDiet(id));
        }
    }
}