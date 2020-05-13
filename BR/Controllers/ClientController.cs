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



        // NEW CLIENT

        // change return type
        [Authorize(Roles = "Admin")]
        [HttpPost("NewByAdmin")]
        public async Task<ActionResult<ServerResponse<ClientShortInfoForAdmin>>> NewClientByAdmin([FromBody]NewClientByAdminRequest newClientRequest)
        {
            string password = _authenticationService.GeneratePassword();
            if (await _userManager.FindByNameAsync(newClientRequest.Email) is null)
            {
                IdentityUser clientIdentity = new IdentityUser()
                {
                    Email = newClientRequest.Email,
                    UserName = newClientRequest.Email
                };

                IdentityResult res = await _userManager.CreateAsync(clientIdentity, password);
                if (res.Succeeded)
                {
                    clientIdentity = await _userManager.FindByNameAsync(newClientRequest.Email);
                    var role = await _roleManager.FindByNameAsync("Client");
                    if (role != null)
                    {
                        var resp = await _userManager.AddToRoleAsync(clientIdentity, "Client");
                        if (!resp.Succeeded)
                        {
                            return new JsonResult(new ServerResponse<ClientShortInfoForAdmin>(Utils.StatusCode.Error, null));
                        }
                    }
                    ServerResponse<ClientShortInfoForAdmin> clientAddResponse;
                    try
                    {
                        var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
                        if (identityUser is null)
                        {
                            return new JsonResult(new ServerResponse<ClientShortInfoForAdmin>(Utils.StatusCode.UserNotFound, null));
                        }

                        var identityRole = await _userManager.GetRolesAsync(identityUser);
                        clientAddResponse = await _clientService.AddNewClientByAdmin(newClientRequest, clientIdentity.Id);

                        if (clientAddResponse.StatusCode != Utils.StatusCode.Ok)
                        {
                            return new JsonResult(clientAddResponse);
                        }
                    }
                    catch
                    {
                        return new JsonResult(new ServerResponse<ClientShortInfoForAdmin>(Utils.StatusCode.Error, null));
                    }
                    try
                    {
                        string msgBody = $"Login: {clientIdentity.Email}\nPassword: {password}";

                        await _emailService.SendAsync(clientIdentity.Email, "Registration info ", msgBody);
                        return new JsonResult(clientAddResponse);
                    }
                    catch
                    {
                        return new JsonResult(new ServerResponse<ClientShortInfoForAdmin>(Utils.StatusCode.SendingMailError, null));
                    }
                }
                else
                {
                    return new JsonResult(new ServerResponse<ClientShortInfoForAdmin>(Utils.StatusCode.Error, null));
                }
            }
            else
            {
                return new JsonResult(new ServerResponse<ClientShortInfoForAdmin>(Utils.StatusCode.EmailUsed, null));
            }
        }


        [Authorize(Roles = "Owner")]
        [HttpPost("NewByOwner")]
        public async Task<ActionResult<ServerResponse<ClientShortInfoForOwners>>> NewClientByOwner([FromBody]NewClientByOwnerRequest newClientRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.UserNotFound, null));
            }
            string password = _authenticationService.GeneratePassword();
            if (await _userManager.FindByNameAsync(newClientRequest.Email) is null)
            {
                IdentityUser clientIdentity = new IdentityUser()
                {
                    Email = newClientRequest.Email,
                    UserName = newClientRequest.Email
                };

                IdentityResult res = await _userManager.CreateAsync(clientIdentity, password);
                if (res.Succeeded)
                {
                    clientIdentity = await _userManager.FindByNameAsync(newClientRequest.Email);
                    var role = await _roleManager.FindByNameAsync("Client");
                    if (role != null)
                    {
                        var resp = await _userManager.AddToRoleAsync(clientIdentity, "Client");
                        if (!resp.Succeeded)
                        {
                            return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.Error, null));
                        }
                    }
                    ServerResponse<ClientShortInfoForOwners> clientAddResponse;
                    try
                    {
                        var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
                        if (identityUser is null)
                        {
                            return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.UserNotFound, null));
                        }

                        var identityRole = await _userManager.GetRolesAsync(identityUser);
                        clientAddResponse = await _clientService.AddNewClientByOwner(newClientRequest, clientIdentity.Id, ownerIdentityUser.Id);

                        if (clientAddResponse.StatusCode != Utils.StatusCode.Ok)
                        {
                            return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(clientAddResponse.StatusCode, null));
                        }
                    }
                    catch
                    {
                        return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.Error, null));
                    }
                    return new JsonResult(clientAddResponse);
                    //try
                    //{
                    //    string msgBody = $"Login: {clientIdentity.Email}\nPassword: {password}";

                    //    await _emailService.SendAsync(clientIdentity.Email, "Registration info ", msgBody);
                    //    
                    //}
                    //catch
                    //{
                    //    return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.Error, null));
                    //}
                }
                else
                {
                    return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.Error, null));
                }
            }
            else
            {
                return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.EmailUsed, null));
            }
        }


        [Authorize(Roles = "Admin")]
        [HttpPut("ConfirmClientRegistration")]
        public async Task<ActionResult<ServerResponse>> ConfirmClientRegistration([FromBody]int clientId)
        {
            var confirmRes = await _clientService.ConfirmClientRegistration(clientId);
            if (confirmRes.StatusCode != Utils.StatusCode.Ok)
            {
                return new JsonResult(Response(confirmRes.StatusCode));
            }

            string password = _authenticationService.GeneratePassword();
            var clientName = await _clientService.GetClientName(clientId);
            if (clientName.StatusCode != Utils.StatusCode.Ok)
            {
                return new JsonResult(Response(clientName.StatusCode));
            }
            var clientIdentity = await _userManager.FindByNameAsync(clientName.Data);

            var _passwordValidator =
                    HttpContext.RequestServices.GetService(typeof(IPasswordValidator<IdentityUser>)) as IPasswordValidator<IdentityUser>;
            var _passwordHasher =
                HttpContext.RequestServices.GetService(typeof(IPasswordHasher<IdentityUser>)) as IPasswordHasher<IdentityUser>;

            IdentityResult result =
                await _passwordValidator.ValidateAsync(_userManager, clientIdentity, password);

            if (result.Succeeded)
            {
                clientIdentity.PasswordHash = _passwordHasher.HashPassword(clientIdentity, password);
                var updateResult = await _userManager.UpdateAsync(clientIdentity);
                if (!updateResult.Succeeded)
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
                    return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.SendingMailError, null));
                }

            }
            else
            {
                return new JsonResult(Response(Utils.StatusCode.Error));
            }

        }


        // UPDATE CLIENT

        [Authorize(Roles = "Admin")]
        [HttpPut("UpdateByAdmin")]
        public async Task<ActionResult<ServerResponse<ClientShortInfoForAdmin>>> UpdateByAdmin([FromBody]UpdateClientRequest updateRequest)
        {
            return new JsonResult(await _clientService.UpdateClientByAdmin(updateRequest));
        }


        [Authorize(Roles = "Owner")]
        [HttpPut("UpdateByOwner")]
        public async Task<ActionResult<ServerResponse<ClientShortInfoForOwners>>> UpdateByOwner([FromBody]UpdateClientRequest updateRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ClientShortInfoForOwners>(Utils.StatusCode.UserNotFound, null));
            }
            return new JsonResult(await _clientService.UpdateClientByOwner(updateRequest, ownerIdentityUser.Id));
        }



        // GET SHORT INFO


        [Authorize(Roles = "Admin")]
        [HttpGet("ShortForAdmin")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForAdmin>>>> ShortForAdmin()
        {
            return new JsonResult(await _clientService.GetShortClientInfoForAdmin());
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("ShortForOwners")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForOwners>>>> ShortForOwner()
        {

            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<ClientShortInfoForOwners>>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _clientService.GetShortClientInfoForOwners(identityUser.Id));
        }



        // FULL INFO

        [Authorize(Roles = "Admin")]
        [HttpGet("FullInfoForAdmin/{id}")]
        public async Task<ActionResult<ServerResponse<ClientFullInfoForAdmin>>> FullInfoForAdmin(int id)
        {
            return new JsonResult(await _clientService.GetFullClientInfoForAdmin(id));
        }


        [Authorize(Roles = "Owner")]
        [HttpGet("FullInfoForOwners/{id}")]
        public async Task<ActionResult<ServerResponse<ClientFullInfoForOwners>>> FullInfoForOwners(int id)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ClientFullInfoForOwners>(Utils.StatusCode.UserNotFound, null));
            }

            return new JsonResult(await _clientService.GetFullClientInfoForOwners(id, ownerIdentityUser.Id));
        }



        // MAIN IMAGE


        [Authorize(Roles = "Admin")]
        [HttpPost("UploadLogoByAdmin")]
        public async Task<ActionResult<ServerResponse<string>>> UploadLogoByAdmin([FromBody]UploadLogoRequest uploadRequest)
        {
            return new JsonResult(await _clientService.UploadLogoByAdmin(uploadRequest));
        }



        [Authorize(Roles = "Owner")]
        [HttpPost("UploadLogoByOwner")]
        public async Task<ActionResult<ServerResponse<string>>> UploadLogoByOwner([FromBody]UploadLogoRequest uploadRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<string>(Utils.StatusCode.UserNotFound, null));
            }
            return new JsonResult(await _clientService.UploadLogoByOwner(uploadRequest, ownerIdentityUser.Id));
        }





        [Authorize(Roles = "Admin")]
        [HttpPut("SetAsMainImageByAdmin")]
        public async Task<ActionResult<ServerResponse>> SetAsMainImageByAdmin([FromBody]int imageId)
        {
            return new JsonResult(await _clientService.SetAsMainImageByAdmin(imageId));
        }


        [Authorize(Roles = "Owner")]
        [HttpPut("SetAsMainImageByOwner")]
        public async Task<ActionResult<ServerResponse>> SetAsMainImageByOwner([FromBody]int imageId)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientService.SetAsMainImageByOwner(imageId, ownerIdentityUser.Id));
        }



        // GALLERY IMAGES

        [Authorize(Roles = "Admin")]
        [HttpPost("UploadImagesByAdmin")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientImageInfo>>>> UploadImagesByAdmin([FromBody]UploadImagesRequest uploadRequest)
        {
            return new JsonResult(await _clientService.UploadImagesByAdmin(uploadRequest));
        }


        [Authorize(Roles = "Owner")]
        [HttpPost("UploadImagesByOwner")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientImageInfo>>>> UploadImagesByOwner([FromBody]UploadImagesRequest uploadRequest)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse<ICollection<ClientImageInfo>>(Utils.StatusCode.UserNotFound, null));
            }
            return new JsonResult(await _clientService.UploadImagesByOwner(uploadRequest, ownerIdentityUser.Id));
        }


        [Authorize(Roles = "Admin")]
        [HttpDelete("DeleteImageByAdmin/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteImageByAdmin(int id)
        {
            return new JsonResult(await _clientService.DeleteImageByAdmin(id));
        }


        [Authorize(Roles = "Owner")]
        [HttpDelete("DeleteImageByOwner/{id}")]
        public async Task<ActionResult<ServerResponse>> DeleteImageByOwner(int id)
        {
            var ownerIdentityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (ownerIdentityUser is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientService.DeleteImageByOwner(id, ownerIdentityUser.Id));
        }


        // BLOCK/UNBLOCK

        [Authorize(Roles = "Admin")]
        [HttpPut("Block")]
        public async Task<ActionResult<ServerResponse>> BlockClient([FromBody]int clientId)
        {
            return new JsonResult(await _clientService.BlockClient(clientId));
        }



        [Authorize(Roles = "Admin")]
        [HttpPut("Unblock")]
        public async Task<ActionResult<ServerResponse>> UnblockClient([FromBody]int clientId)
        {
            return new JsonResult(await _clientService.UnblockClient(clientId));
        }


        // FOR USERS


        [Authorize(Roles = "User")]
        [HttpGet("ByFiltersForUsers")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForUsers>>>> GetClientsByFiltersForUsers(ClientFilter filter, int skip, int take)
        {
            return new JsonResult(await _clientService.GetClientsByFilterForUsers(filter, skip, take));
        }



        [Authorize(Roles = "User")]
        [HttpGet("FullForUsers/{id}")]
        public async Task<ActionResult<ServerResponse<ClientFullInfoForUsers>>> FullInfoForUsers(int id)
        {
            return new JsonResult(Response(await _clientService.GetFullClientInfoForUsers(id)));
        }


        [Authorize(Roles = "User")]
        [HttpGet("FavouriteIds")]
        public async Task<ActionResult<ServerResponse>> GetFavouritesIds()
        {
            var userIdentity = await _userManager.FindByNameAsync(User.Identity.Name);
            if (userIdentity is null)
            {
                return new JsonResult(new ServerResponse(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientService.GetFavouritesIds(userIdentity.Id));
        }


        [Authorize(Roles = "User")]
        [HttpPost("Favourite")]
        public async Task<ActionResult<ServerResponse>> AddFavourite([FromBody]int clientId)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientService.AddFavourite(clientId, identityUser.Id));
        }


        [Authorize(Roles = "User")]
        [HttpDelete("Favourite")]
        public async Task<ActionResult<ServerResponse>> DeleteFavourite(int clientId)
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }
            return new JsonResult(await _clientService.DeleteFavourite(clientId, identityUser.Id));
        }


        [Authorize(Roles = "User")]
        [HttpGet("ComingSoon")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForUsers>>>> GetComingSoon(int skip, int take)
        {
            return new JsonResult(await _clientService.GetComingSoon(skip, take));
        }





        // ================================================================================================





        [HttpGet("ShortForUsers")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForUsers>>>> ShortForUsers()
        {
            return new JsonResult(Response(await _clientService.GetShortClientInfoForUsers()));
        }





        [HttpGet("Favourite")]
        public async Task<ActionResult<ServerResponse<ICollection<ClientShortInfoForUsers>>>> Favourite()
        {
            var identityUser = await _userManager.FindByNameAsync(User.Identity.Name);
            if (identityUser is null)
            {
                return new JsonResult(Response(Utils.StatusCode.UserNotFound));
            }

            return new JsonResult(Response(await _clientService.GetFavourites(identityUser.Id)));
        }







        [HttpGet("mealtype")]
        public async Task<ActionResult<ServerResponse<IEnumerable<ClientFullInfoForUsers>>>> Get(string mealType)
        {
            return new JsonResult(Response(await _clientService.GetClientsByMeal(mealType)));
        }


        [HttpGet("search")]
        public async Task<ActionResult<ServerResponse<IEnumerable<ClientFullInfoForUsers>>>> Search(string name)
        {
            return new JsonResult(Response(await _clientService.GetClientsByName(name)));
        }





        [HttpGet("schema/{id}")]
        public async Task<ActionResult<ServerResponse<ClientHallsInfo>>> ClientSchema(int id)
        {
            return new JsonResult(Response(await _clientService.GetClientHalls(id)));
        }







        // DONE
        [HttpPut("Delete")]
        public async Task<ActionResult<ServerResponse>> Delete(int id)
        {
            return new JsonResult(await _clientService.DeleteClient(id));
        }



    }
}