using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Clients;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BR.Services
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly IBlobService _blobService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IClientService _clientService;

        public ClientAccountService(IAsyncRepository repository,
            IBlobService blobService,
            IAuthenticationService authenticationService,
            IClientService clientService)
        {
            _repository = repository;
            _blobService = blobService;
            _authenticationService = authenticationService;
            _clientService = clientService;
        }

        public async Task<ServerResponse<LogInResponse>> LogIn(string userName, string notificationTag)
        {
            return await _authenticationService.Authentication(userName, notificationTag);
        }



        public async Task LogOut(string notificationTag)
        {
            AccountToken accountToken = await _repository.GetTokenByTag(notificationTag);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }




        public async Task<ServerResponse<ClientFullInfoForClients>> GetProfileInfo(string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ClientFullInfoForClients>(StatusCode.UserNotFound, null);
                }

                var images = new List<ClientImageInfo>();
                foreach (var item in client.ClientImages)
                {
                    images.Add(new ClientImageInfo()
                    {
                        Id = item.Id,
                        Path = item.ImagePath
                    });
                }

                var cuisines = new List<int>();
                foreach (var item in client.ClientCuisines)
                {
                    cuisines.Add(item.CuisineId);
                }

                var clientTypes = new List<int>();
                foreach (var item in client.ClientClientTypes)
                {
                    clientTypes.Add(item.ClientTypeId);
                }

                var mealTypes = new List<int>();
                foreach (var item in client.ClientMealTypes)
                {
                    mealTypes.Add(item.MealTypeId);
                }

                var goodFors = new List<int>();
                foreach (var item in client.ClientGoodFors)
                {
                    goodFors.Add(item.GoodForId);
                }

                var features = new List<int>();
                foreach (var item in client.ClientFeatures)
                {
                    features.Add(item.FeatureId);
                }

                var dishes = new List<int>();
                foreach (var item in client.ClientDishes)
                {
                    dishes.Add(item.DishId);
                }

                var specialDiets = new List<int>();
                foreach (var item in client.ClientSpecialDiets)
                {
                    specialDiets.Add(item.SpecialDietId);
                }

                var socLinks = new List<string>();
                foreach (var item in client.SocialLinks)
                {
                    socLinks.Add(item.Link);
                }

                var phones = new List<ClientPhoneInfo>();
                foreach (var item in client.ClientPhones)
                {
                    phones.Add(new ClientPhoneInfo()
                    {
                        Number = item.Number,
                        IsWhatsApp = item.IsWhatsApp
                    });
                }


                return new ServerResponse<ClientFullInfoForClients>(StatusCode.Ok,
                    new ClientFullInfoForClients()
                    {
                        ClientName = client.RestaurantName,
                        BarReserveDurationAvg = client.BarReserveDurationAvg,
                        ReserveDurationAvg = client.ReserveDurationAvg,
                        RegistrationDate = client.RegistrationDate,
                        PriceCategory = client.PriceCategory,
                        OrganizationName = client.Organization.Title,
                        CloseTime = client.CloseTime,
                        OpenTime = client.OpenTime,
                        ConfirmationDuration = client.ConfirmationDuration,
                        Description = client.Description,
                        Email = client.Identity.Email,
                        Images = images,
                        Lat = client.Lat,
                        Long = client.Long,
                        MainImagePath = client.MainImagePath,
                        OrganizationId = client.OrganizationId,
                        MaxReserveDays = client.MaxReserveDays,
                        ClientTypeIds = clientTypes,
                        CuisineIds = cuisines,
                        DishIds = dishes,
                        FeatureIds = features,
                        GoodForIds = goodFors,
                        MealTypeIds = mealTypes,
                        Phones = phones,
                        SocialLinks = socLinks,
                        SpecialDietIds = specialDiets
                    });
            }
            catch
            {
                return new ServerResponse<ClientFullInfoForClients>(StatusCode.DbConnectionError, null);
            }
        }




        public async Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken)
        {
            return await _authenticationService.UpdateToken(refreshToken);
        }

        public async Task<ServerResponse<bool>> ClientIsBlocked(string identityId)
        {
            var client = await _repository.GetClient(identityId);
            if (client is null)
            {
                return new ServerResponse<bool>(StatusCode.UserNotFound, false);
            }
            if (client.Blocked is null)
            {
                return new ServerResponse<bool>(StatusCode.Ok, false);
            }
            else
            {
                return new ServerResponse<bool>(StatusCode.Ok, true);
            }
        }

        public async Task<ServerResponse<bool>> ClientIsDeleted(string identityId)
        {
            var client = await _repository.GetClient(identityId);
            if (client is null)
            {
                return new ServerResponse<bool>(StatusCode.UserNotFound, false);
            }
            if (client.Deleted is null)
            {
                return new ServerResponse<bool>(StatusCode.Ok, false);
            }
            else
            {
                return new ServerResponse<bool>(StatusCode.Ok, true);
            }
        }



        // change
        public async Task<ServerResponse<string>> UploadMainImage(string identityId, string imageString)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse<string>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            return await _clientService.UploadMainImageByAdmin(new UploadMainImageRequest()
            {
                ClientId = client.Id,
                ImageString = imageString
            });
        }


        // chnage
        public async Task<ServerResponse> UploadImages(string identityId, ICollection<string> imagesString)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }

            return await _clientService.UploadImagesByAdmin(new UploadImagesRequest()
            {
                ClientId = client.Id,
                ImageStrings = imagesString
            });
        }


        // change
        public async Task<ServerResponse> SetAsMainImage(int imageId)
        {
            return new ServerResponse(StatusCode.Ok);
            //return await _clientService.SetAsMainImage(imageId);
        }


        // change
        public async Task<ServerResponse> DeleteImage(int imageId)
        {
            return new ServerResponse(StatusCode.Ok); 
            //return await _clientService.DeleteImage(imageId);
        }

        // change

        public async Task<ServerResponse> UpdateClient(UpdateClientRequest updateRequest, string identityIdClient)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityIdClient);
                if (client is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            updateRequest.ClientId = client.Id;

            return new ServerResponse(StatusCode.Ok);
            //return await _clientService.UpdateClient(updateRequest);

        }

    }
}
