﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System.Drawing;
using System.Drawing.Imaging;
using BR.Services.Interfaces;
using BR.DTO.Clients;
using BR.DTO.Users;
using BR.DTO.Events;
using BR.DTO.Schema;
using Microsoft.EntityFrameworkCore;

namespace BR.Services
{

    public class ClientService : IClientService
    {
        private readonly IAsyncRepository _repository;
        private readonly IBlobService _blobService;
        private readonly IReservationService _reservationService;

        public ClientService(IAsyncRepository repository,
            IReservationService reservationService,
            IBlobService blobService)
        {
            _repository = repository;
            _reservationService = reservationService;
            _blobService = blobService;
        }

        public async Task<ServerResponse> AddNewClientByAdmin(NewClientByAdminRequest newRequest, string clientIdentityId)
        {


            Client client = new Client()
            {
                RestaurantName = newRequest.RestaurantName,
                Lat = newRequest.Lat,
                Long = newRequest.Long,
                OpenTime = newRequest.OpenTime,
                CloseTime = newRequest.CloseTime,
                Description = newRequest.Description,
                MaxReserveDays = newRequest.MaxReserveDays,
                ReserveDurationAvg = newRequest.ReserveDurationAvg,
                BarReserveDurationAvg = newRequest.BarReserveDurationAvg,
                ConfirmationDuration = newRequest.ConfirmationDuration,
                PriceCategory = newRequest.PriceCategory,
                IdentityId = clientIdentityId,
                RegistrationDate = DateTime.Now,
                OrganizationId = newRequest.OrganizationId,
                IsConfirmedByAdmin = true
            };


            if (newRequest.MainImage is null)
            {
                client.MainImagePath = "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg";
            }
            else
            {
                try
                {
                    client.MainImagePath = await _blobService.UploadImage(newRequest.MainImage);
                }
                catch
                {
                    return new ServerResponse(StatusCode.BlobError);
                }
            }



            try
            {
                Client addedClient = await _repository.AddClient(client);

                await this.AddClientsParameters(client.Id, newRequest.CuisineIds, newRequest.ClientTypeIds, newRequest.MealTypeIds, newRequest.DishIds, newRequest.GoodForIds, newRequest.SpecialDietIds, newRequest.FeatureIds, newRequest.Phones, newRequest.SocialLinks);


                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }


        }


        public async Task<ServerResponse<ClientShortInfoForOwnersResponse>> AddNewClientByOwner(NewClientByOwnerRequest newRequest, string clientIdentityId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<ClientShortInfoForOwnersResponse>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwnersResponse>(StatusCode.DbConnectionError, null);
            }

            Client client = new Client()
            {
                RestaurantName = newRequest.RestaurantName,
                Lat = newRequest.Lat,
                Long = newRequest.Long,
                OpenTime = newRequest.OpenTime,
                CloseTime = newRequest.CloseTime,
                Description = newRequest.Description,
                MaxReserveDays = newRequest.MaxReserveDays,
                ReserveDurationAvg = newRequest.ReserveDurationAvg,
                BarReserveDurationAvg = newRequest.BarReserveDurationAvg,
                ConfirmationDuration = newRequest.ConfirmationDuration,
                PriceCategory = newRequest.PriceCategory,
                IdentityId = clientIdentityId,
                RegistrationDate = DateTime.Now,
                OrganizationId = owner.OrganizationId,
                IsConfirmedByAdmin = false
            };

            if (newRequest.MainImage is null)
            {
                client.MainImagePath = "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg";
            }
            else
            {
                try
                {
                    client.MainImagePath = await _blobService.UploadImage(newRequest.MainImage);
                }
                catch
                {
                    return new ServerResponse<ClientShortInfoForOwnersResponse>(StatusCode.BlobError, null);
                }
            }



            try
            {
                Client addedClient = await _repository.AddClient(client);

                await this.AddClientsParameters(client.Id, newRequest.CuisineIds, newRequest.ClientTypeIds, newRequest.MealTypeIds, newRequest.DishIds, newRequest.GoodForIds, newRequest.SpecialDietIds, newRequest.FeatureIds, newRequest.Phones, newRequest.SocialLinks);


                return new ServerResponse<ClientShortInfoForOwnersResponse>(StatusCode.Ok, new ClientShortInfoForOwnersResponse()
                {
                    Id = addedClient.Id,
                    Blocked = addedClient.Blocked,
                    Deleted = addedClient.Deleted,
                    ClientName = addedClient.RestaurantName,
                    Email = addedClient.Identity.Email,
                    MainImagePath = addedClient.MainImagePath,
                    RegistrationDate = addedClient.RegistrationDate
                });
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwnersResponse>(StatusCode.Error, null);
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
                                IsWhatsApp = phone.IsWhatsApp
                            });
                    }
                    catch { }
                }
            }
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



        public async Task<ICollection<ClientShortInfoForUsersResponse>> GetShortClientInfoForUsers()
        {
            var clients = await _repository.GetClients();
            if (clients is null)
            {
                return null;
            }
            var res = new List<ClientShortInfoForUsersResponse>();
            foreach (var client in clients)
            {
                res.Add(new ClientShortInfoForUsersResponse()
                {
                    Id = client.Id,
                    Name = client.RestaurantName,
                    MainImage = client.MainImagePath,
                    RegistrationDate = client.RegistrationDate
                });
            }
            return res;

        }

        public async Task<ServerResponse<ICollection<ClientShortInfoForAdminResponse>>> GetShortClientInfoForAdmin()
        {
            IEnumerable<Client> clients;
            try
            {
                clients = await _repository.GetClients();

                if (clients is null)
                {
                    return new ServerResponse<ICollection<ClientShortInfoForAdminResponse>>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientShortInfoForAdminResponse>>(StatusCode.DbConnectionError, null);
            }
            var res = new List<ClientShortInfoForAdminResponse>();
            foreach (var client in clients)
            {
                res.Add(new ClientShortInfoForAdminResponse()
                {
                    Id = client.Id,
                    ClientName = client.RestaurantName,
                    Email = client.Identity.Email,
                    OrganizationName = client.Organization.Title,
                    MainImagePath = client.MainImagePath,
                    RegistrationDate = client.RegistrationDate,
                    Blocked = client.Blocked,
                    Deleted = client.Deleted
                });
            }
            return new ServerResponse<ICollection<ClientShortInfoForAdminResponse>>(StatusCode.Ok, res);

        }


        public async Task<ServerResponse<ICollection<ClientShortInfoForOwnersResponse>>> GetShortClientInfoForOwners(string ownerIdentityId)
        {
            Owner owner;
            try
            {

                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null || owner.Organization.Clients is null)
                {
                    return new ServerResponse<ICollection<ClientShortInfoForOwnersResponse>>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientShortInfoForOwnersResponse>>(StatusCode.DbConnectionError, null);
            }
            var res = new List<ClientShortInfoForOwnersResponse>();
            foreach (var client in owner.Organization.Clients)
            {
                res.Add(new ClientShortInfoForOwnersResponse()
                {
                    Id = client.Id,
                    ClientName = client.RestaurantName,
                    Email = client.Identity.Email,
                    MainImagePath = client.MainImagePath,
                    RegistrationDate = client.RegistrationDate,
                    Blocked = client.Blocked,
                    Deleted = client.Deleted
                });
            }
            return new ServerResponse<ICollection<ClientShortInfoForOwnersResponse>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ClientFullInfoForAdminResponse>> GetFullClientInfoForAdmin(int id)
        {
            Client client;
            try
            {

                client = await _repository.GetClient(id);
                if (client is null)
                {
                    return new ServerResponse<ClientFullInfoForAdminResponse>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientFullInfoForAdminResponse>(StatusCode.DbConnectionError, null);
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
            var clientInfo = new ClientFullInfoForAdminResponse()
            {
                Id = client.Id,
                ClientName = client.RestaurantName,
                OrganizationId = client.OrganizationId,
                OrganizationName = client.Organization.Title,
                MainImagePath = client.MainImagePath,
                Email = client.Identity.Email,
                RegistrationDate = client.RegistrationDate,
                Blocked = client.Blocked,
                Deleted = client.Deleted,
                OpenTime = client.OpenTime,
                CloseTime = client.CloseTime,
                Description = client.Description,
                Lat = client.Lat,
                Long = client.Long,
                MaxReserveDays = client.MaxReserveDays,
                ReserveDurationAvg = client.ReserveDurationAvg,
                BarReserveDurationAvg = client.BarReserveDurationAvg,
                PriceCategory = client.PriceCategory,
                ConfirmationDuration = client.ConfirmationDuration,
                ClientTypeIds = this.ClientTypesToIdsList(client.ClientClientTypes),
                CuisineIds = this.CuisinesToIdsList(client.ClientCuisines),
                MealTypeIds = this.MealTypesToIdsList(client.ClientMealTypes),
                DishIds = this.DishesToIdsList(client.ClientDishes),
                FeatureIds = this.FeaturesToIdsList(client.ClientFeatures),
                GoodForIds = this.GoodForsToIdsList(client.ClientGoodFors),
                SpecialDietIds = this.SpecialDietsToIdsList(client.ClientSpecialDiets),
                Phones = this.PhonesToList(client.ClientPhones),
                SocialLinks = this.SocialLinksToList(client.SocialLinks),
                Images = images
            };

            return new ServerResponse<ClientFullInfoForAdminResponse>(StatusCode.Ok, clientInfo);
        }


        public async Task<ServerResponse<ClientFullInfoForOwnersResponse>> GetFullClientInfoForOwners(int clientId, string ownerIdentityId)
        {
            Owner owner;
            try
            {

                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<ClientFullInfoForOwnersResponse>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientFullInfoForOwnersResponse>(StatusCode.DbConnectionError, null);
            }
            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);
            if(client is null)
            {
                return new ServerResponse<ClientFullInfoForOwnersResponse>(StatusCode.NotFound, null);
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
            var clientInfo = new ClientFullInfoForOwnersResponse()
            {
                Id = client.Id,
                ClientName = client.RestaurantName,
                OrganizationId = client.OrganizationId,
                OrganizationName = client.Organization.Title,
                MainImagePath = client.MainImagePath,
                Email = client.Identity.Email,
                RegistrationDate = client.RegistrationDate,
                Blocked = client.Blocked,
                Deleted = client.Deleted,
                OpenTime = client.OpenTime,
                CloseTime = client.CloseTime,
                Description = client.Description,
                Lat = client.Lat,
                Long = client.Long,
                MaxReserveDays = client.MaxReserveDays,
                ReserveDurationAvg = client.ReserveDurationAvg,
                BarReserveDurationAvg = client.BarReserveDurationAvg,
                PriceCategory = client.PriceCategory,
                ConfirmationDuration = client.ConfirmationDuration,
                ClientTypeIds = this.ClientTypesToIdsList(client.ClientClientTypes),
                CuisineIds = this.CuisinesToIdsList(client.ClientCuisines),
                MealTypeIds = this.MealTypesToIdsList(client.ClientMealTypes),
                DishIds = this.DishesToIdsList(client.ClientDishes),
                FeatureIds = this.FeaturesToIdsList(client.ClientFeatures),
                GoodForIds = this.GoodForsToIdsList(client.ClientGoodFors),
                SpecialDietIds = this.SpecialDietsToIdsList(client.ClientSpecialDiets),
                Phones = this.PhonesToList(client.ClientPhones),
                SocialLinks = this.SocialLinksToList(client.SocialLinks),
                Images = images
            };

            return new ServerResponse<ClientFullInfoForOwnersResponse>(StatusCode.Ok, clientInfo);
        }




        public async Task<ClientFullInfoForUsersResponse> GetFullClientInfoForUsers(int id)
        {
            Client client = await _repository.GetClient(id);

            if (client is null)
            {
                return null;
            }
            return this.ClientToFullInfoForUsers(client);
        }

        public async Task<ICollection<ClientFullInfoForUsersResponse>> GetFullClientInfoForUsers()
        {
            var clients = await _repository.GetClients();
            if (clients is null)
            {
                return null;
            }
            var res = new List<ClientFullInfoForUsersResponse>();
            foreach (var item in clients)
            {
                res.Add(this.ClientToFullInfoForUsers(item));
            }
            return res;
        }

        public async Task<ICollection<ClientShortInfoForUsersResponse>> GetFavourites(string identityUserId)
        {
            var user = await _repository.GetUser(identityUserId);
            if (user is null)
            {
                return null;
            }
            var favourites = user.ClientFavourites;
            if (favourites is null)
            {
                return null;
            }
            var res = new List<ClientShortInfoForUsersResponse>();
            foreach (var fav in favourites)
            {
                res.Add(new ClientShortInfoForUsersResponse()
                {
                    Id = fav.Client.Id,
                    MainImage = fav.Client.MainImagePath,
                    Name = fav.Client.RestaurantName,
                    RegistrationDate = fav.Client.RegistrationDate
                });
            }
            return res;
        }

        public async Task<ServerResponse> AddFavourite(int clientId, string identityUserId)
        {
            User user;
            try
            {

                user = await _repository.GetUser(identityUserId);
                if (user is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            var clientFav = new ClientFavourite()
            {
                ClientId = clientId,
                UserId = user.Id
            };
            try
            {
                var res = await _repository.AddFavourite(clientFav);
                return new ServerResponse(StatusCode.Ok);
            }
            catch (DbUpdateException)
            {
                return new ServerResponse(StatusCode.Duplicate);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }

        }

        public async Task<ServerResponse> DeleteFavourite(int clientId, string identityUserId)
        {

            User user;
            ClientFavourite favourite;
            try
            {
                user = await _repository.GetUser(identityUserId);
                if (user is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
                favourite = await _repository.GetFavourite(clientId, user.Id);
                if (favourite is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            try
            {
                await _repository.DeleteFavourite(favourite);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

        }

        public async Task<ICollection<ClientFullInfoForUsersResponse>> GetClientsByMeal(string mealType)
        {
            var clients = await _repository.GetClientsByMeal(mealType);
            if (clients != null)
            {
                var res = new List<ClientFullInfoForUsersResponse>();
                foreach (var client in clients)
                {
                    res.Add(this.ClientToFullInfoForUsers(client));
                }
                return res;
            }
            return null;
        }

        public async Task<IEnumerable<ClientFullInfoForUsersResponse>> GetClientsByName(string title)
        {
            var clients = await _repository.GetClientsByName(title);
            if (clients != null)
            {
                var res = new List<ClientFullInfoForUsersResponse>();
                foreach (var client in clients)
                {
                    res.Add(this.ClientToFullInfoForUsers(client));
                }
                return res;
            }
            return null;

        }

        public async Task<ClientFullInfoForAdminResponse> GetClientForAdmin(int id)
        {
            var client = await _repository.GetClient(id);
            if (client is null)
            {
                return null;
            }
            return new ClientFullInfoForAdminResponse()
            {
                //Id = client.Id,
                //Name = client.RestaurantName,
                //MainImage = client.MainImagePath,
                //Address = client.Address,
                //Email = client.Identity.Email,
                //RegistrationDate = client.RegistrationDate,
                //IsBlocked = client.IsBlocked,
                //OpenTime = client.OpenTime,
                //CloseTime = client.CloseTime,
                //AdditionalInfo = client.Description,
                //IsBusinessLunch = client.IsBusinessLunch,
                //IsChildrenZone = client.IsChildrenZone,
                //IsLiveMusic = client.IsLiveMusic,
                //IsOpenSpace = client.IsOpenSpace,
                //IsParking = client.IsParking,
                //IsWiFi = client.IsWiFi,
                //Lat = client.Lat,
                //Long = client.Long,
                //MaxReserveDays = client.MaxReserveDays,
                //IsBarReservation = client.BarReserveDurationAvg is null ? false : true,
                //ReserveDurationAvg = client.ReserveDurationAvg,
                //ConfirmationDuration = client.ConfirmationDuration,
                //ClientTypes = this.ClientTypesToList(client.ClientClientTypes),
                //Cuisines = this.CuisinesToList(client.ClientCuisines),
                //Events = this.EventsToList(client.Events),
                //Phones = this.PhonesToList(client.ClientPhones),
                //SocialLinks = this.SocialLinksToList(client.SocialLinks),
                //MealTypes = this.MealTypesToList(client.ClientMealTypes),
                //Photos = this.ImagesToList(client.ClientImages)
            };
        }

        public async Task<ClientHallsInfoResponse> GetClientHalls(int id)
        {
            var client = await _repository.GetClient(id);
            if (client is null)
            {
                return null;
            }
            var floorsInfo = new List<FloorInfo>();

            foreach (var floor in client.Floors)
            {
                var hallsInfo = new List<HallInfo>();
                foreach (var hall in floor.Halls)
                {
                    hallsInfo.Add(new HallInfo()
                    {
                        Title = hall.Title,
                        JsonInfo = hall.JsonInfo
                    });
                }
                floorsInfo.Add(new FloorInfo()
                {
                    Number = floor.Number,
                    Halls = hallsInfo
                });
            }
            return new ClientHallsInfoResponse()
            {
                ClientId = id,
                Floors = floorsInfo
            };
        }


        public async Task<ServerResponse> UpdateClient(UpdateClientRequest updateRequest)
        {
            if (updateRequest.ClientId is null)
            {
                return new ServerResponse(StatusCode.Error);
            }

            Client client;
            try
            {
                client = await _repository.GetClient(updateRequest.ClientId ?? default);
                if (client is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.UserNotFound);
            }


            client.RestaurantName = updateRequest.RestaurantName;
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
                return new ServerResponse(StatusCode.Duplicate);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }

            await this.RemoveClientsParameters(client.ClientCuisines, client.ClientClientTypes, client.ClientMealTypes, client.ClientDishes, client.ClientGoodFors, client.ClientSpecialDiets, client.ClientFeatures, client.ClientPhones, client.SocialLinks);

            await this.AddClientsParameters(client.Id, updateRequest.CuisineIds, updateRequest.ClientTypeIds, updateRequest.MealTypeIds, updateRequest.DishIds, updateRequest.GoodForIds, updateRequest.SpecialDietIds, updateRequest.FeatureIds, updateRequest.Phones, updateRequest.SocialLinks);

            return new ServerResponse(StatusCode.Ok);
        }


        public async Task<ServerResponse<string>> UploadMainImage(UploadMainImageRequest uploadRequest)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(uploadRequest.ClientId);
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
                client.MainImagePath = await _blobService.UploadImage(uploadRequest.ImageString);

            }
            catch
            {
                return new ServerResponse<string>(StatusCode.BlobError, null);
            }
            try
            {
                await _repository.UpdateClient(client);
                return new ServerResponse<string>(StatusCode.Ok, client.MainImagePath);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<ServerResponse> SetAsMainImage(int imageId)
        {
            ClientImage clientImage;
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

            var client = clientImage.Client;

            try
            {
                await _blobService.DeleteImage(client.MainImagePath);
            }
            catch
            {
                return new ServerResponse(StatusCode.BlobError);
            }

            client.MainImagePath = clientImage.ImagePath;
            try
            {
                await _repository.UpdateClient(client);
                await _repository.DeleteClientImage(clientImage);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }


        public async Task<ServerResponse<ICollection<ClientImageInfo>>> UploadImages(UploadImagesRequest uploadRequest)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(uploadRequest.ClientId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.DbConnectionError, null);
            }

            ICollection<ClientImage> clientImages = new List<ClientImage>();
            try
            {
                foreach (var item in uploadRequest.ImageStrings)
                {
                    clientImages.Add(new ClientImage()
                    {
                        ClientId = client.Id,
                        ImagePath = await _blobService.UploadImage(item)
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
                var cl = await _repository.GetClient(uploadRequest.ClientId);
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
                            Path = item.ImagePath
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
            ClientImage clientImage;
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


        public async Task<ServerResponse> BlockClient(int clientId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientId);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
            if (client is null)
            {
                return new ServerResponse(StatusCode.UserNotFound);
            }
            if (client.Deleted != null)
            {
                return new ServerResponse(StatusCode.UserDeleted);
            }
            if (client.Blocked is null)
            {
                client.Blocked = DateTime.Now;
                try
                {
                    await _repository.UpdateClient(client);

                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }

                // delete tokens
                var tokens = await _repository.GetTokens(client.IdentityId);
                if (tokens != null)
                {
                    foreach (var item in tokens)
                    {
                        await _repository.RemoveToken(item);
                    }
                }

                // cancel upcoming reservations
                var reservations = client.Reservations;
                var cancelReason = await _repository.GetCancelReason("ClientBlocked");

                // if closes admin
                // var admin = (await _repository.GetAdmins()).FirstOrDefault();
                if (cancelReason != null && reservations != null)
                {
                    foreach (var item in reservations)
                    {
                        if (item.ReservationDate > DateTime.Now && item.ReservationStateId is null)
                        {
                            await _reservationService.CancelReservation(item.Id, cancelReason.Id, client.IdentityId);
                        }
                    }
                }

                return new ServerResponse(StatusCode.Ok);

            }
            else
            {
                return new ServerResponse(StatusCode.UserBlocked);
            }
        }

        public async Task<ServerResponse> UnblockClient(int clientId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientId);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
            if (client is null)
            {
                return new ServerResponse(StatusCode.UserNotFound);
            }
            if (client.Deleted != null)
            {
                return new ServerResponse(StatusCode.UserDeleted);
            }
            if (client.Blocked != null)
            {
                client.Blocked = null;
                try
                {
                    await _repository.UpdateClient(client);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.UserUnblocked);
            }
        }


        public async Task<ServerResponse> ConfirmClient(int clientId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientId);
                if (client is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            if (client.IsConfirmedByAdmin)
            {
                return new ServerResponse(StatusCode.Error);
            }
            client.IsConfirmedByAdmin = true;
            try
            {
                await _repository.UpdateClient(client);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
        }



        private ClientFullInfoForUsersResponse ClientToFullInfoForUsers(Client client)
        {
            if (client is null)
            {
                return null;
            }
            return new ClientFullInfoForUsersResponse()
            {
                //Id = client.Id,
                //Name = client.RestaurantName,
                //Address = client.Address,
                //Lat = client.Lat,
                //Long = client.Long,
                //OpenTime = client.OpenTime,
                //CloseTime = client.CloseTime,
                //IsBusinessLunch = client.IsBusinessLunch,
                //IsChildrenZone = client.IsChildrenZone,
                //IsLiveMusic = client.IsLiveMusic,
                //IsOpenSpace = client.IsOpenSpace,
                //IsParking = client.IsParking,
                //IsWiFi = client.IsWiFi,
                //MaxReserveDays = client.MaxReserveDays,
                //IsBarReservation = client.BarReserveDurationAvg is null ? false : true,
                //ReserveDurationAvg = client.ReserveDurationAvg,
                //ConfirmationDuration = client.ConfirmationDuration,
                //MainImage = client.MainImagePath,
                //ClientTypes = this.ClientTypesToList(client.ClientClientTypes),
                //Cuisines = this.CuisinesToList(client.ClientCuisines),
                //MealTypes = this.MealTypesToList(client.ClientMealTypes),
                //Photos = this.ImagesToList(client.ClientImages),
                //Phones = this.PhonesToList(client.ClientPhones),
                //SocialLinks = this.SocialLinksToList(client.SocialLinks),
                //Events = this.EventsToList(client.Events)
            };
        }



        private ICollection<string> CuisinesToList(ICollection<ClientCuisine> cuisines)
        {
            if (cuisines is null)
            {
                return null;
            }
            var res = new List<string>();
            foreach (var item in cuisines)
            {
                res.Add(item.Cuisine.Title);
            }
            return res;
        }



        private ICollection<string> ClientTypesToList(ICollection<ClientClientType> clientTypes)
        {
            if (clientTypes is null)
            {
                return null;
            }
            var res = new List<string>();
            foreach (var item in clientTypes)
            {
                res.Add(item.ClientType.Title);
            }
            return res;
        }

        private ICollection<string> MealTypesToList(ICollection<ClientMealType> mealTypes)
        {
            if (mealTypes is null)
            {
                return null;
            }
            var res = new List<string>();
            foreach (var item in mealTypes)
            {
                res.Add(item.MealType.Title);
            }
            return res;
        }

        private ICollection<int> CuisinesToIdsList(ICollection<ClientCuisine> cuisines)
        {
            if (cuisines is null)
            {
                return null;
            }
            var res = new List<int>();
            foreach (var item in cuisines)
            {
                res.Add(item.CuisineId);
            }
            return res;
        }

        private ICollection<int> ClientTypesToIdsList(ICollection<ClientClientType> clientTypes)
        {
            if (clientTypes is null)
            {
                return null;
            }
            var res = new List<int>();
            foreach (var item in clientTypes)
            {
                res.Add(item.ClientTypeId);
            }
            return res;
        }

        private ICollection<int> MealTypesToIdsList(ICollection<ClientMealType> mealTypes)
        {
            if (mealTypes is null)
            {
                return null;
            }
            var res = new List<int>();
            foreach (var item in mealTypes)
            {
                res.Add(item.MealTypeId);
            }
            return res;
        }

        private ICollection<int> GoodForsToIdsList(ICollection<ClientGoodFor> goodFors)
        {
            if (goodFors is null)
            {
                return null;
            }
            var res = new List<int>();
            foreach (var item in goodFors)
            {
                res.Add(item.GoodForId);
            }
            return res;
        }

        private ICollection<int> SpecialDietsToIdsList(ICollection<ClientSpecialDiet> specialDiets)
        {
            if (specialDiets is null)
            {
                return null;
            }
            var res = new List<int>();
            foreach (var item in specialDiets)
            {
                res.Add(item.SpecialDietId);
            }
            return res;
        }

        private ICollection<int> DishesToIdsList(ICollection<ClientDish> dishes)
        {
            if (dishes is null)
            {
                return null;
            }
            var res = new List<int>();
            foreach (var item in dishes)
            {
                res.Add(item.DishId);
            }
            return res;
        }


        private ICollection<int> FeaturesToIdsList(ICollection<ClientFeature> features)
        {
            if (features is null)
            {
                return null;
            }
            var res = new List<int>();
            foreach (var item in features)
            {
                res.Add(item.FeatureId);
            }
            return res;
        }


        private ICollection<string> SocialLinksToList(ICollection<SocialLink> socialLinks)
        {
            if (socialLinks is null)
            {
                return null;
            }
            var res = new List<string>();
            foreach (var item in socialLinks)
            {
                res.Add(item.Link);
            }
            return res;
        }

        private ICollection<ClientPhoneInfo> PhonesToList(ICollection<ClientPhone> phones)
        {
            if (phones is null)
            {
                return null;
            }
            var res = new List<ClientPhoneInfo>();
            foreach (var item in phones)
            {
                res.Add(new ClientPhoneInfo()
                {
                    Number = item.Number,
                    IsWhatsApp = item.IsWhatsApp
                });
            }
            return res;
        }

        private ICollection<string> ImagesToList(ICollection<ClientImage> images)
        {
            if (images is null)
            {
                return null;
            }
            var res = new List<string>();
            foreach (var item in images)
            {
                res.Add(item.ImagePath);
            }
            return res;
        }

        private ICollection<EventInfo> EventsToList(ICollection<Event> events)
        {
            if (events is null)
            {
                return null;
            }
            var res = new List<EventInfo>();
            foreach (var item in events)
            {
                res.Add(new EventInfo()
                {
                    Id = item.Id,
                    Date = item.Date,
                    Description = item.Description,
                    ImgPath = item.ImagePath,
                    Title = item.Title
                });
            }
            return res;
        }


        public async Task<ServerResponse> DeleteClient(int clientId)
        {
            var client = await _repository.GetClient(clientId);
            if (client != null)
            {
                client.Deleted = DateTime.Now;
                try
                {
                    await _repository.UpdateClient(client);

                    // delete tokens
                    var tokens = await _repository.GetTokens(client.IdentityId);
                    if (tokens != null)
                    {
                        foreach (var item in tokens)
                        {
                            await _repository.RemoveToken(item);
                        }
                    }

                    // cancel upcoming reservations
                    var reservations = client.Reservations;
                    var cancelReason = await _repository.GetCancelReason("ClientDeleted");

                    // if closes admin
                    //var admin = (await _repository.GetAdmins()).FirstOrDefault();
                    if (cancelReason != null && reservations != null)
                    {
                        foreach (var item in reservations)
                        {
                            if (item.ReservationDate > DateTime.Now && item.ReservationStateId is null)
                            {
                                await _reservationService.CancelReservation(item.Id, cancelReason.Id, client.IdentityId);
                            }
                        }
                    }

                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            return new ServerResponse(StatusCode.UserNotFound);
        }

    }
}
