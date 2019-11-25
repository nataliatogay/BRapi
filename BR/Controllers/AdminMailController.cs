using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.Models;
using BR.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BR.Controllers
{
    /*
    [Route("api/[controller]")]
    [ApiController]
    public class AdminMailController : ControllerBase
    {
        private readonly IAdminMailService _adminMailService;
        private readonly IEmailService _emailService;
        public AdminMailController(IAdminMailService adminMailService, IEmailService emailService)
        {
            _adminMailService = adminMailService;
            _emailService = emailService;
        }
        [HttpGet("/user")]
        public async Task<ActionResult<IEnumerable<UserMail>>> GetUserMails()
        {
            return new JsonResult((await _adminMailService.GetAllUserMails(User.Identity.Name)).ToList());
        }

        [HttpGet("/user/{id}")] 
        public async Task<IActionResult> GetUserMail(int id)
        {
            return new JsonResult(await _adminMailService.GetUserMail(id));
        }

        [HttpGet("/client")]
        public async Task<ActionResult<IEnumerable<UserMail>>> GetClientMails()
        {
            return new JsonResult((await _adminMailService.GetAllClientMails(User.Identity.Name)).ToList());
        }

        [HttpPost("user/send")]
        public async Task<IActionResult> SendUserMail([FromBody]SendMailRequest sendMailRequest)
        {
            string recipent = await _adminMailService.GetUserEmail(sendMailRequest.RecipentId);
            try
            {               
                await _emailService.SendAsync(recipent, sendMailRequest.Subject, sendMailRequest.Body);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            await _adminMailService.SendUserMail(User.Identity.Name, sendMailRequest);
            return Ok();
        }

        [HttpGet("/client/{id}")]
        public async Task<IActionResult> GetClientMail(int id)
        {
            return new JsonResult(await _adminMailService.GetClientMail(id));
        }

        [HttpPost("client/send")]
        public async Task<IActionResult> SendClientMail([FromBody]SendMailRequest sendMailRequest)
        {
            string recipent = await _adminMailService.GetClientEmail(sendMailRequest.RecipentId);
            try
            {
                await _emailService.SendAsync(recipent, sendMailRequest.Subject, sendMailRequest.Body);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            await _adminMailService.SendClientMail(User.Identity.Name, sendMailRequest);
            return Ok();
        }
    }
    */
}