using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SchemaController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ISchemaService _schemaService;

        public SchemaController(UserManager<IdentityUser> userManager,
            ISchemaService schemaService)
        {
            _userManager = userManager;
            _schemaService = schemaService;
        }

        [HttpPost("")]
        public async Task<IActionResult> Post([FromBody]NewSchemaRequest newSchema)
        {

            //var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //if (identityUser is null)
            //{
            //    return new JsonResult("Client not found");
            //}
            //await _schemaService.AddNewSchema(newScheme, identityUser.Id);

            await _schemaService.AddNewSchema(newSchema, "123");
            return Ok();
        }

        [HttpPut("")]
        public async Task<IActionResult> Put([FromBody]UpdateSchemaRequest updateSchema)
        {

            //var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            //if (identityUser is null)
            //{
            //    return new JsonResult("Client not found");
            //}
            //await _schemaService.AddNewSchema(newSchema, identityUser.Id);

           await _schemaService.UpdateSchema(updateSchema);
            return Ok();
        }
    }
}