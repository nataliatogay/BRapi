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
using Microsoft.EntityFrameworkCore;
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
                        Path = item.ImagePath,
                        IsMain = item.IsMain

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
                        IsWhatsApp = item.IsWhatsApp,
                        IsTelegram = item.IsTelegram
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
                        AdminName = client.AdminName,
                        AdminPhoneNumber = client.AdminPhoneNumber,
                        LogoPath = client.LogoPath,
                        Images = images,
                        Lat = client.Lat,
                        Long = client.Long,
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


        public async Task<ServerResponse<string>> GetClientName(int clientId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<string>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }

            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);
            if (client is null)
            {
                return new ServerResponse<string>(StatusCode.NotFound, null);
            }
            return new ServerResponse<string>(StatusCode.Ok, client.Identity.UserName);
        }


        public async Task<ServerResponse> UpdateClient(UpdateClientProfileRequest updateRequest, string identityIdClient)
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

            if (updateRequest.LogoString != null)
            {

                // delete previous logo from blob
                //try
                //{
                //    await _blobService.DeleteImage(client.LogoPath);
                //}
                //catch { }

                try
                {
                    client.LogoPath = await _blobService.UploadImage(updateRequest.LogoString);
                }
                catch
                {
                    return new ServerResponse<ClientShortInfoForOwners>(StatusCode.BlobError, null);
                }
            }

            client.RestaurantName = updateRequest.RestaurantName.Trim();
            client.Lat = updateRequest.Lat;
            client.Long = updateRequest.Long;
            client.OpenTime = updateRequest.OpenTime;
            client.CloseTime = updateRequest.CloseTime;
            client.Description = updateRequest.Description;
            client.MaxReserveDays = updateRequest.MaxReserveDays;
            client.ReserveDurationAvg = updateRequest.ReserveDurationAvg;
            client.BarReserveDurationAvg = updateRequest.BarReserveDurationAvg;
            client.ConfirmationDuration = updateRequest.ConfirmationDuration;
            client.PriceCategory = updateRequest.PriceCategory;

            try
            {
                await _repository.UpdateClient(client);
            }
            catch (DbUpdateException)
            {
                return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.Duplicate, null);
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.Error, null);
            }

            await this.RemoveClientsParameters(client.ClientCuisines, client.ClientClientTypes, client.ClientMealTypes, client.ClientDishes, client.ClientGoodFors, client.ClientSpecialDiets, client.ClientFeatures, client.ClientPhones, client.SocialLinks);

            await this.AddClientsParameters(client.Id, updateRequest.CuisineIds, updateRequest.ClientTypeIds, updateRequest.MealTypeIds, updateRequest.DishIds, updateRequest.GoodForIds, updateRequest.SpecialDietIds, updateRequest.FeatureIds, updateRequest.Phones, updateRequest.SocialLinks);


            return new ServerResponse(StatusCode.Ok);

        }

        private async Task RemoveClientsParameters(ICollection<ClientCuisine> cuisines, ICollection<ClientClientType> clientTypes, ICollection<ClientMealType> mealTypes, ICollection<ClientDish> dishes, ICollection<ClientGoodFor> goodFors, ICollection<ClientSpecialDiet> specialDiets, ICollection<ClientFeature> features, ICollection<ClientPhone> phones, ICollection<SocialLink> socialLinks)
        {

            if (cuisines != null)
            {
                try
                {
                    await _repository.RemoveClientCuisine(cuisines);
                }
                catch { }
            }

            if (clientTypes != null)
            {

                try
                {
                    await _repository.RemoveClientClientType(clientTypes);
                }
                catch { }
            }

            if (mealTypes != null)
            {
                try
                {
                    await _repository.RemoveClientMealType(mealTypes);
                }
                catch { }
            }

            if (dishes != null)
            {
                try
                {
                    await _repository.RemoveClientDish(dishes);
                }
                catch { }
            }

            if (goodFors != null)
            {
                try
                {
                    await _repository.RemoveClientGoodFor(goodFors);
                }
                catch { }
            }

            if (specialDiets != null)
            {
                try
                {
                    await _repository.RemoveClientSpecialDiet(specialDiets);
                }
                catch { }
            }

            if (features != null)
            {
                try
                {
                    await _repository.RemoveClientFeature(features);
                }
                catch { }
            }

            if (socialLinks != null)
            {

                try
                {
                    await _repository.RemoveClientSocialLink(socialLinks);
                }
                catch { }
            }

            if (phones != null)
            {
                try
                {
                    await _repository.RemoveClientPhone(phones);
                }
                catch { }
            }
        }

        private async Task AddClientsParameters(int clientId, ICollection<int> cuisineIds, ICollection<int> clientTypeIds, ICollection<int> mealTypeIds, ICollection<int> dishIds, ICollection<int> goodForIds, ICollection<int> specialDietIds, ICollection<int> featureIds, ICollection<ClientPhoneInfo> phones, ICollection<string> socialLinks)
        {

            if (mealTypeIds != null)
            {
                foreach (var item in mealTypeIds)
                {
                    try
                    {
                        await _repository.AddClientMealType(
                            new ClientMealType()
                            {
                                ClientId = clientId,
                                MealTypeId = item
                            });
                    }
                    catch { }
                }
            }

            if (clientTypeIds != null)
            {
                foreach (var clientTypeId in clientTypeIds)
                {
                    try
                    {
                        await _repository.AddClientClientType(new ClientClientType()
                        {
                            ClientId = clientId,
                            ClientTypeId = clientTypeId
                        });
                    }
                    catch { }
                }
            }

            if (cuisineIds != null)
            {
                foreach (var cuisineId in cuisineIds)
                {
                    try
                    {
                        await _repository.AddClientCuisine(new ClientCuisine()
                        {
                            ClientId = clientId,
                            CuisineId = cuisineId
                        });
                    }
                    catch { }
                }
            }

            if (dishIds != null)
            {
                foreach (var dishId in dishIds)
                {
                    try
                    {
                        await _repository.AddClientDish(new ClientDish()
                        {
                            ClientId = clientId,
                            DishId = dishId
                        });
                    }
                    catch { }
                }
            }

            if (goodForIds != null)
            {
                foreach (var goodForId in goodForIds)
                {
                    try
                    {
                        await _repository.AddClientGoodFor(new ClientGoodFor()
                        {
                            ClientId = clientId,
                            GoodForId = goodForId
                        });
                    }
                    catch { }
                }
            }

            if (specialDietIds != null)
            {
                foreach (var dietId in specialDietIds)
                {
                    try
                    {
                        await _repository.AddClientSpecialDiet(new ClientSpecialDiet()
                        {
                            ClientId = clientId,
                            SpecialDietId = dietId
                        });
                    }
                    catch { }
                }
            }

            if (featureIds != null)
            {
                foreach (var featureId in featureIds)
                {
                    try
                    {
                        await _repository.AddClientFeature(new ClientFeature()
                        {
                            ClientId = clientId,
                            FeatureId = featureId
                        });
                    }
                    catch { }
                }
            }

            if (socialLinks != null)
            {
                foreach (var link in socialLinks)
                {
                    try
                    {
                        await _repository.AddClientSocialLink(new SocialLink()
                        {
                            ClientId = clientId,
                            Link = link.Trim()
                        });
                    }
                    catch { }
                }
            }

            if (phones != null)
            {
                foreach (var phone in phones)
                {
                    try
                    {
                        await _repository.AddClientPhone(
                            new ClientPhone()
                            {
                                ClientId = clientId,
                                Number = phone.Number,
                                IsWhatsApp = phone.IsWhatsApp,
                                IsTelegram = phone.IsTelegram
                            });
                    }
                    catch { }
                }
            }
        }




        public async Task<ServerResponse<LogInClientOwnerResponse>> UpdateToken(string refreshToken)
        {
            var authResponse = await _authenticationService.UpdateToken(refreshToken);
            if (authResponse.StatusCode != StatusCode.Ok)
            {
                return new ServerResponse<LogInClientOwnerResponse>(authResponse.StatusCode, null);
            }

            AccountToken token;
            try
            {
                token = await _repository.GetToken(refreshToken);
                if (token is null)
                {
                    return new ServerResponse<LogInClientOwnerResponse>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<LogInClientOwnerResponse>(StatusCode.DbConnectionError, null);
            }
            var roleResp = await _authenticationService.GetRoles(token.IdentityUserId);
            if(roleResp.StatusCode != StatusCode.Ok)
            {
                return new ServerResponse<LogInClientOwnerResponse>(roleResp.StatusCode, null);
            }

            return new ServerResponse<LogInClientOwnerResponse>(StatusCode.Ok,
                new LogInClientOwnerResponse()
                {
                    AccessToken = authResponse.Data.AccessToken,
                    RefreshToken = authResponse.Data.RefreshToken,
                    Role = roleResp.Data[0]
                });
        }



        public async Task<ServerResponse<bool>> ClientIsConfirmed(string identityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse<bool>(StatusCode.UserNotFound, false);
                }
            }
            catch
            {
                return new ServerResponse<bool>(StatusCode.DbConnectionError, false);
            }

            if (client.AdminConfirmation is null)
            {
                return new ServerResponse<bool>(StatusCode.Ok, false);
            }
            else
            {
                return new ServerResponse<bool>(StatusCode.Ok, true);
            }
        }

        public async Task<ServerResponse<bool>> ClientIsBlocked(string identityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse<bool>(StatusCode.UserNotFound, false);
                }
            }
            catch
            {
                return new ServerResponse<bool>(StatusCode.DbConnectionError, false);
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
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse<bool>(StatusCode.UserNotFound, false);
                }
            }
            catch
            {
                return new ServerResponse<bool>(StatusCode.DbConnectionError, false);
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


        public async Task<ServerResponse<string>> UploadLogo(string identityId, string logoString)
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

            try
            {
                client.LogoPath = await _blobService.UploadImage(logoString);

            }
            catch
            {
                return new ServerResponse<string>(StatusCode.BlobError, null);
            }
            try
            {
                await _repository.UpdateClient(client);
                return new ServerResponse<string>(StatusCode.Ok, client.LogoPath);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }


        }

        public async Task<ServerResponse> SetAsMainImage(int imageId)
        {
            ClientGalleryImage clientImage;
            try
            {
                clientImage = await _repository.GetClientImage(imageId);
                if (clientImage is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            var mainImage = clientImage.Client.ClientImages.FirstOrDefault(item => item.IsMain);
            if (mainImage != null)
            {
                mainImage.IsMain = false;
                try
                {
                    await _repository.UpdateClientImage(mainImage);
                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }
            }

            var client = clientImage.Client;

            clientImage.IsMain = true;

            try
            {
                clientImage = await _repository.UpdateClientImage(clientImage);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

        }


        public async Task<ServerResponse<ICollection<ClientImageInfo>>> UploadImages(string identityId, ICollection<string> imagesString)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.DbConnectionError, null);
            }

            ICollection<ClientGalleryImage> clientImages = new List<ClientGalleryImage>();
            try
            {
                foreach (var item in imagesString)
                {
                    clientImages.Add(new ClientGalleryImage()
                    {
                        ClientId = client.Id,
                        ImagePath = await _blobService.UploadImage(item),
                        IsMain = false
                    });
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.BlobError, null);
            }
            try
            {
                await _repository.AddClientImages(clientImages);
                if (client is null)
                {
                    return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.UserNotFound, null);
                }
                var images = new List<ClientImageInfo>();
                if (client != null)
                {
                    foreach (var item in client.ClientImages)
                    {
                        images.Add(new ClientImageInfo()
                        {
                            Id = item.Id,
                            Path = item.ImagePath,
                            IsMain = item.IsMain
                        });
                    }
                }
                return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.Ok, images);
            }
            catch
            {
                return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse> DeleteImage(int imageId)
        {
            ClientGalleryImage clientImage;
            try
            {
                clientImage = await _repository.GetClientImage(imageId);
                if (clientImage is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
                await _repository.DeleteClientImage(clientImage);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            try
            {
                await _blobService.DeleteImage(clientImage.ImagePath);
            }
            catch { }
            return new ServerResponse(StatusCode.Ok);
        }


        // ===========================================================================================



    }
}
