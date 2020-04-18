using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Clients;
using BR.DTO.Schema;
using BR.DTO.Users;
using BR.Models;
using BR.Services;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BR.Controllers
{
    // [Authorize]
    // [Authorize(Roles = "Admin, User")]
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ResponseController
    {
        private readonly IClientService _clientService;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IAuthenticationService _authenticationService;


        public ClientController(IClientService clientService,
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService,
            IAuthenticationService authenticationService)
        {
            _clientService = clientService;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
            _authenticationService = authenticationService;

        }



        // by owner and admin
        [HttpPost("")]
        public async Task<ActionResult<ServerResponse>> Post([FromBody]NewClientRequest newClient)
        {



            string password = _authenticationService.GeneratePassword();
            if (await _userManager.FindByNameAsync(newClient.Email) is null)
            {
                IdentityUser clientIdentity = new IdentityUser()
                {
                    Email = newClient.Email,
                    UserName = newClient.Email
                };

                IdentityResult res = await _userManager.CreateAsync(clientIdentity, password);
                if (res.Succeeded)
                {
                    clientIdentity = await _userManager.FindByNameAsync(newClient.Email);
                    var role = await _roleManager.FindByNameAsync("Client");
                    if (role != null)
                    {
                        var resp = await _userManager.AddToRoleAsync(clientIdentity, "Client");
                        if (!resp.Succeeded)
                        {
                            return new JsonResult(Response(Utils.StatusCode.Error));
                        }
                    }
                    try
                    {
                        var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
                        if (identityUser is null)
                        {
                            return new JsonResult(Response(Utils.StatusCode.UserNotFound));
                        }

                        var identityRole = await _userManager.GetRolesAsync(identityUser);
                        ServerResponse clientAddResponse = await _clientService.AddNewClient(newClient, clientIdentity.Id, identityUser.Id, identityRole.FirstOrDefault());

                        if (clientAddResponse.StatusCode != Utils.StatusCode.Ok)
                        {
                            return new JsonResult(clientAddResponse);
                        }
                    }
                    catch
                    {
                        return new JsonResult(Response(Utils.StatusCode.Error));
                    }
                    try
                    {
                        string msgBody = $"Login: {clientIdentity.Email}\nPassword: {password}";

                        await _emailService.SendAsync(clientIdentity.Email, "Registration info ", msgBody);
                        return new JsonResult(Response(Utils.StatusCode.Ok));
                    }
                    catch
                    {
                        return new JsonResult(Response(Utils.StatusCode.Error));
                    }
                }
                else
                {
                    return new JsonResult(Response(Utils.StatusCode.Error));
                }
            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.EmailUsed));
            }
        }








        [HttpGet("ShortForUsers")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForUsersResponse>>>> ShortForUsers()
        {
            return new JsonResult(Response(await _clientService.GetShortClientInfoForUsers()));
        }

        [HttpGet("ShortForAdmin")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForAdminResponse>>>> ShortForAdmin()
        {
            return new JsonResult(await _clientService.GetShortClientInfoForAdmin());
        }


        [HttpGet("Favourite")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForUsersResponse>>>> Favourite()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(Response(await _clientService.GetFavourites(identityUser.Id)));
        }

        [HttpPost("Favourite")]
        public async Task<ActionResult<ServerResponse>> AddFavourite(int clientId)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientService.AddFavourite(clientId, identityUser.Id));
        }

        [HttpDelete("Favourite")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForUsersResponse>>>> DeleteFavourite(int clientId)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientService.DeleteFavourite(clientId, identityUser.Id));
        }




        [HttpGet("mealtype")]
        public async Task<ActionResult<ServerResponse<IEnumerable<ClientFullInfoForUsersResponse>>>> Get(string mealType)
        {
            return new JsonResult(Response(await _clientService.GetClientsByMeal(mealType)));
        }


        [HttpGet("search")]
        public async Task<ActionResult<ServerResponse<IEnumerable<ClientFullInfoForUsersResponse>>>> Search(string name)
        {
            return new JsonResult(Response(await _clientService.GetClientsByName(name)));
        }


        [HttpGet("ForAdmin/{id}")]
        public async Task<ActionResult<ServerResponse<ClientFullInfoForAdminResponse>>> FullInfoForAdmin(int id)
        {
            return new JsonResult(await _clientService.GetFullClientInfoForAdmin(id));
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<ServerResponse<ClientFullInfoForAdminResponse>>> FullInfoForUser(int id)
        {
            return new JsonResult(Response(await _clientService.GetFullClientInfoForUsers(id)));
        }


        [HttpGet("schema/{id}")]
        public async Task<ActionResult<ServerResponse<ClientHallsInfoResponse>>> ClientSchema(int id)
        {
            return new JsonResult(Response(await _clientService.GetClientHalls(id)));
        }




        



        // by owner, admin
        [HttpPut("")]
        public async Task<ActionResult<ServerResponse<Client>>> Update([FromBody]UpdateClientRequest updateRequest)
        {
            return new JsonResult(Response(await _clientService.UpdateClient(updateRequest)));
        }


        // by owner, admin
        // [Authorize]
        [HttpPost("UploadMainImage")]
        public async Task<ActionResult<ServerResponse<string>>> UploadMainImage([FromBody]UploadMainImageRequest uploadRequest)
        {
            return new JsonResult(await _clientService.UploadMainImage(uploadRequest));
        }


        // by owner, admin
        // [Authorize]
        [HttpPut("SetAsMainImage")]
        public async Task<ActionResult<ServerResponse>> SetAsMainImage([FromBody]int imageId)
        {
            return new JsonResult(await _clientService.SetAsMainImage(imageId));
        }


        // by owner, admin
        [HttpPut("UploadImages")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientImageInfo>>>> UploadImages([FromBody]UploadImagesRequest uploadRequest)
        {
            return new JsonResult(await _clientService.UploadImages(uploadRequest));
        }


        // by owner, admin
        [HttpDelete("DeleteImage/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteImage(int id)
        {
            return new JsonResult(await _clientService.DeleteImage(id));
        }


        // by admin
        [HttpPut("Block")]
        public async Task<ActionResult<ServerResponse>> BlockClient([FromBody]int clientId)
        {
            return new JsonResult(await _clientService.BlockClient(clientId));
        }



        // by admin
        [HttpPut("Unblock")]
        public async Task<ActionResult<ServerResponse>> UnblockClient([FromBody]int clientId)
        {
            return new JsonResult(await _clientService.UnblockClient(clientId));
        }



        // by admin
        [HttpPut("Confirm")]
        public async Task<ActionResult<ServerResponse>> ConfirmClient([FromBody]int clientId)
        {
            return new JsonResult(await _clientService.UnblockClient(clientId));
        }

        // DONE
        [HttpPut("Delete")]
        public async Task<ActionResult<ServerResponse>> Delete(int id)
        {
            return new JsonResult(await _clientService.DeleteClient(id));
        }



    }
}