using System;
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
using BR.DTO.Organization;
using BR.DTO.Parameters;

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

        public async Task<ServerResponse<ClientShortInfoForAdmin>> AddNewClientByAdmin(NewClientByAdminRequest newRequest, string clientIdentityId)
        {
            Client client = new Client()
            {
                RestaurantName = newRequest.RestaurantName.Trim(),
                AdminName = newRequest.AdminName.Trim(),
                AdminPhoneNumber = newRequest.AdminPhoneNumber,
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
                AdminConfirmation = DateTime.Now,
                Blocked = null,
                Deleted = null
            };


            try
            {
                client.LogoPath = await _blobService.UploadImage(newRequest.LogoString);
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.BlobError, null);
            }

            try
            {
                client = await _repository.AddClient(client);

                await this.AddClientsParameters(client.Id, newRequest.CuisineIds, newRequest.ClientTypeIds, newRequest.MealTypeIds, newRequest.DishIds, newRequest.GoodForIds, newRequest.SpecialDietIds, newRequest.FeatureIds, newRequest.Phones, newRequest.SocialLinks);


                // CHANGE

                return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.Ok,
                   new ClientShortInfoForAdmin()
                   {
                       //Id = client.Id,
                       //Blocked = client.Blocked,
                       //ClientName = client.RestaurantName,
                       //Deleted = client.Deleted,
                       //Email = client.Identity.Email,
                       //MainImagePath = client.MainImagePath,
                       //OrganizationName = client.Organization.Title,
                       //RegistrationDate = client.RegistrationDate
                   });


            }
            catch (DbUpdateException ex)
            {
                return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.Duplicate, null);
            }

            catch
            {
                return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.Error, null);
            }

        }


        public async Task<ServerResponse<ClientShortInfoForOwners>> AddNewClientByOwner(NewClientByOwnerRequest newRequest, string clientIdentityId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<ClientShortInfoForOwners>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.DbConnectionError, null);
            }

            Client client = new Client()
            {
                RestaurantName = newRequest.RestaurantName,
                AdminName = newRequest.AdminName,
                AdminPhoneNumber = newRequest.AdminPhoneNumber,
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
                AdminConfirmation = null
            };


            // добавление Logo 
            if (newRequest.LogoString is null)
            {
                client.LogoPath = "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg";
            }
            else
            {
                try
                {
                    client.LogoPath = await _blobService.UploadImage(newRequest.LogoString);
                }
                catch
                {
                    return new ServerResponse<ClientShortInfoForOwners>(StatusCode.BlobError, null);
                }
            }
            Client addedClient;

            try
            {
                addedClient = await _repository.AddClient(client);

                await this.AddClientsParameters(client.Id, newRequest.CuisineIds, newRequest.ClientTypeIds, newRequest.MealTypeIds, newRequest.DishIds, newRequest.GoodForIds, newRequest.SpecialDietIds, newRequest.FeatureIds, newRequest.Phones, newRequest.SocialLinks);


            }
            catch (DbUpdateException)
            {
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.Duplicate, null);
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.Error, null);
            }


            NotificationType type;
            try
            {

                type = await _repository.GetNotificationType("registration");
                if (type is null)
                {
                    return new ServerResponse<ClientShortInfoForOwners>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.DbConnectionError, null);
            }

            AdminNotification adminNotification = new AdminNotification()
            {
                ClientId = client.Id,
                NotificationTypeId = type.Id,
                DateTime = DateTime.Now,
                Done = null,
                Title = "New Client Was Registered"
            };
            try
            {
                adminNotification = await _repository.AddAdminNotification(adminNotification);

                // CHANGE
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.Ok, new ClientShortInfoForOwners()
                {
                    Id = addedClient.Id,
                    Blocked = addedClient.Blocked,
                    Deleted = addedClient.Deleted,
                    ClientName = addedClient.RestaurantName,
                    Email = addedClient.Identity.Email,
                    MainImagePath = addedClient.LogoPath,
                    RegistrationDate = addedClient.RegistrationDate,
                    Confirmed = addedClient.AdminConfirmation,
                    LogoPath = addedClient.LogoPath
                });
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.DbConnectionError, null);
            }


        }

        public async Task<ServerResponse> ConfirmClientRegistration(int clientId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientId);
                if (client is null)
                {
                    return new ServerResponse(StatusCode.NotFound);

                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            client.AdminConfirmation = DateTime.Now;
            try
            {
                client = await _repository.UpdateClient(client);
                return new ServerResponse(StatusCode.Ok);
            }
            catch (DbUpdateException)
            {
                return new ServerResponse(StatusCode.Duplicate);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
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


        public async Task<ServerResponse<string>> GetClientName(int clientId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientId);
                if (client is null)
                {
                    return new ServerResponse<string>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            return new ServerResponse<string>(StatusCode.Ok, client.Identity.UserName);
        }


        public async Task<ICollection<ClientShortInfoForUsers>> GetShortClientInfoForUsers()
        {
            var clients = await _repository.GetClients();
            if (clients is null)
            {
                return null;
            }
            var res = new List<ClientShortInfoForUsers>();
            foreach (var client in clients)
            {
                // change
                res.Add(new ClientShortInfoForUsers()
                {
                    //Id = client.Id,
                    //Name = client.RestaurantName,
                    //MainImage = client.ClientImages.FirstOrDefault(item => item.IsMain) is null ? "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg" : client.ClientImages.FirstOrDefault(item => item.IsMain).ImagePath,
                });
            }
            return res;

        }

        public async Task<ServerResponse<ICollection<ClientShortInfoForAdmin>>> GetShortClientInfoForAdmin()
        {
            IEnumerable<Client> clients;
            try
            {
                clients = await _repository.GetClients();

                if (clients is null)
                {
                    return new ServerResponse<ICollection<ClientShortInfoForAdmin>>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientShortInfoForAdmin>>(StatusCode.DbConnectionError, null);
            }
            var res = new List<ClientShortInfoForAdmin>();
            foreach (var client in clients)
            {
                res.Add(new ClientShortInfoForAdmin()
                {
                    Id = client.Id,
                    ClientName = client.RestaurantName,
                    Email = client.Identity.Email,
                    OrganizationName = client.Organization.Title,
                    MainImagePath = client.ClientImages.FirstOrDefault(item => item.IsMain) is null ? "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg" : client.ClientImages.FirstOrDefault(item => item.IsMain).ImagePath,
                    RegistrationDate = client.RegistrationDate,
                    Blocked = client.Blocked,
                    Deleted = client.Deleted
                });
            }
            return new ServerResponse<ICollection<ClientShortInfoForAdmin>>(StatusCode.Ok, res);

        }


        public async Task<ServerResponse<ICollection<ClientShortInfoForOwners>>> GetShortClientInfoForOwners(string ownerIdentityId)
        {
            Owner owner;
            try
            {

                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null || owner.Organization.Clients is null)
                {
                    return new ServerResponse<ICollection<ClientShortInfoForOwners>>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientShortInfoForOwners>>(StatusCode.DbConnectionError, null);
            }
            var res = new List<ClientShortInfoForOwners>();
            foreach (var client in owner.Organization.Clients)
            {
                res.Add(new ClientShortInfoForOwners()
                {
                    Id = client.Id,
                    ClientName = client.RestaurantName,
                    Email = client.Identity.Email,
                    LogoPath = client.LogoPath,
                    MainImagePath = client.ClientImages.FirstOrDefault(item => item.IsMain) is null ? "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg" : client.ClientImages.FirstOrDefault(item => item.IsMain).ImagePath,
                    RegistrationDate = client.RegistrationDate,
                    Confirmed = client.AdminConfirmation,
                    Blocked = client.Blocked,
                    Deleted = client.Deleted
                });
            }
            return new ServerResponse<ICollection<ClientShortInfoForOwners>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ClientFullInfoForAdmin>> GetFullClientInfoForAdmin(int id)
        {
            Client client;
            try
            {

                client = await _repository.GetClient(id);
                if (client is null)
                {
                    return new ServerResponse<ClientFullInfoForAdmin>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientFullInfoForAdmin>(StatusCode.DbConnectionError, null);
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
            var clientInfo = new ClientFullInfoForAdmin()
            {
                Id = client.Id,
                ClientName = client.RestaurantName,
                Organization = new OrganizationInfo()
                {
                    Id = client.OrganizationId,
                    Title = client.Organization.Title,
                    LogoPath = client.Organization.LogoPath
                },

                Email = client.Identity.Email,
                AdminName = client.AdminName,
                AdminPhoneNumber = client.AdminPhoneNumber,
                RegistrationDate = client.RegistrationDate,
                Confirmed = client.AdminConfirmation,
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

            return new ServerResponse<ClientFullInfoForAdmin>(StatusCode.Ok, clientInfo);
        }


        public async Task<ServerResponse<ClientFullInfoForOwners>> GetFullClientInfoForOwners(int clientId, string ownerIdentityId)
        {
            Owner owner;
            try
            {

                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<ClientFullInfoForOwners>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientFullInfoForOwners>(StatusCode.DbConnectionError, null);
            }
            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);
            if (client is null)
            {
                return new ServerResponse<ClientFullInfoForOwners>(StatusCode.NotFound, null);
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

            var clientInfo = new ClientFullInfoForOwners()
            {
                Id = client.Id,
                ClientName = client.RestaurantName,
                OrganizationId = client.OrganizationId,
                OrganizationName = client.Organization.Title,
                LogoPath = client.LogoPath,
                Email = client.Identity.Email,
                AdminName = client.AdminName,
                AdminPhoneNumber = client.AdminPhoneNumber,
                RegistrationDate = client.RegistrationDate,
                Confirmed = client.AdminConfirmation,
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

            return new ServerResponse<ClientFullInfoForOwners>(StatusCode.Ok, clientInfo);
        }


        public async Task<ServerResponse<ClientShortInfoForAdmin>> UpdateClientByAdmin(UpdateClientRequest updateRequest)
        {

            Client client;
            try
            {
                client = await _repository.GetClient(updateRequest.ClientId);
                if (client is null)
                {
                    return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.UserNotFound, null);
            }

            client.RestaurantName = updateRequest.RestaurantName.Trim();
            client.AdminName = updateRequest.AdminName.Trim();
            client.AdminPhoneNumber = updateRequest.AdminPhoneNumber;
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

            return new ServerResponse<ClientShortInfoForAdmin>(StatusCode.Ok,
                new ClientShortInfoForAdmin()
                {
                    Id = client.Id,
                    RegistrationDate = client.RegistrationDate,
                    MainImagePath = client.ClientImages.FirstOrDefault(item => item.IsMain) is null ? "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg" : client.ClientImages.FirstOrDefault(item => item.IsMain).ImagePath,
                    Email = client.Identity.Email,
                    Blocked = client.Blocked,
                    ClientName = client.RestaurantName,
                    OrganizationName = client.Organization.Title,
                    Deleted = client.Deleted
                });
        }




        public async Task<ServerResponse<ClientShortInfoForOwners>> UpdateClientByOwner(UpdateClientRequest updateRequest, string ownerIdentityId)
        {
            Owner owner;
            Client client;
            try
            {

                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<ClientShortInfoForOwners>(StatusCode.UserNotFound, null);
                }
                client = await _repository.GetClient(updateRequest.ClientId);
                if (owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(client))
                {
                    return new ServerResponse<ClientShortInfoForOwners>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.UserNotFound, null);
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
            client.AdminName = updateRequest.AdminName.Trim();
            client.AdminPhoneNumber = updateRequest.AdminPhoneNumber;
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
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.Duplicate, null);
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForOwners>(StatusCode.Error, null);
            }

            await this.RemoveClientsParameters(client.ClientCuisines, client.ClientClientTypes, client.ClientMealTypes, client.ClientDishes, client.ClientGoodFors, client.ClientSpecialDiets, client.ClientFeatures, client.ClientPhones, client.SocialLinks);

            await this.AddClientsParameters(client.Id, updateRequest.CuisineIds, updateRequest.ClientTypeIds, updateRequest.MealTypeIds, updateRequest.DishIds, updateRequest.GoodForIds, updateRequest.SpecialDietIds, updateRequest.FeatureIds, updateRequest.Phones, updateRequest.SocialLinks);

            return new ServerResponse<ClientShortInfoForOwners>(StatusCode.Ok,
                new ClientShortInfoForOwners()
                {
                    Id = client.Id,
                    Blocked = client.Blocked,
                    ClientName = client.RestaurantName,
                    Deleted = client.Deleted,
                    Confirmed = client.AdminConfirmation,
                    Email = client.Identity.Email,
                    LogoPath = client.LogoPath,
                    MainImagePath = client.ClientImages.FirstOrDefault(item => item.IsMain) is null ? "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg" : client.ClientImages.FirstOrDefault(item => item.IsMain).ImagePath,
                    RegistrationDate = client.RegistrationDate
                });
        }



        public async Task<ServerResponse<string>> UploadLogoByAdmin(UploadLogoRequest uploadRequest)
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
                client.LogoPath = await _blobService.UploadImage(uploadRequest.LogoString);

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



        public async Task<ServerResponse<string>> UploadLogoByOwner(UploadLogoRequest uploadRequest, string ownerIdentityId)
        {
            Owner owner;
            Client client;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);

                if (owner is null)
                {
                    return new ServerResponse<string>(StatusCode.UserNotFound, null);
                }

                client = await _repository.GetClient(uploadRequest.ClientId);
                if (owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(client))
                {
                    return new ServerResponse<string>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            try
            {
                client.LogoPath = await _blobService.UploadImage(uploadRequest.LogoString);

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


        public async Task<ServerResponse> SetAsMainImageByAdmin(int imageId)
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


        public async Task<ServerResponse> SetAsMainImageByOwner(int imageId, string ownerIdentityId)
        {
            Owner owner;
            ClientGalleryImage clientImage;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
                clientImage = await _repository.GetClientImage(imageId);
                if (clientImage is null || owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(clientImage.Client))
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


        public async Task<ServerResponse<ICollection<ClientImageInfo>>> UploadImagesByAdmin(UploadImagesRequest uploadRequest)
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

            ICollection<ClientGalleryImage> clientImages = new List<ClientGalleryImage>();
            try
            {
                foreach (var item in uploadRequest.ImageStrings)
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
                //var cl = await _repository.GetClient(uploadRequest.ClientId);
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


        public async Task<ServerResponse<ICollection<ClientImageInfo>>> UploadImagesByOwner(UploadImagesRequest uploadRequest, string ownerIdentityId)
        {
            Owner owner;
            Client client;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.UserNotFound, null);
                }
                client = await _repository.GetClient(uploadRequest.ClientId);
                if (client is null || owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(client))
                {
                    return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ClientImageInfo>>(StatusCode.DbConnectionError, null);
            }

            ICollection<ClientGalleryImage> clientImages = new List<ClientGalleryImage>();
            try
            {
                foreach (var item in uploadRequest.ImageStrings)
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


        public async Task<ServerResponse> DeleteImageByAdmin(int imageId)
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

        public async Task<ServerResponse> DeleteImageByOwner(int imageId, string ownerIdentityId)
        {
            Owner owner;
            ClientGalleryImage clientImage;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }

                clientImage = await _repository.GetClientImage(imageId);
                if (clientImage is null || owner.Organization is null || owner.Organization.Clients is null || !owner.Organization.Clients.Contains(clientImage.Client))
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


        public async Task<ServerResponse<string>> DeleteUnconfirmedClientByOwner(int clientId, string ownerIdentityId)
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
            if (client.AdminConfirmation != null)
            {
                return new ServerResponse<string>(StatusCode.Error, null);
            }
            try
            {
                await this.RemoveClientsParameters(client.ClientCuisines, client.ClientClientTypes, client.ClientMealTypes, client.ClientDishes, client.ClientGoodFors, client.ClientSpecialDiets, client.ClientFeatures, client.ClientPhones, client.SocialLinks);
                AdminNotification notif = client.AdminNotification;
                if (notif != null)
                {
                    notif.ClientId = null;
                    await _repository.UpdateAdminNotification(notif);
                }
                await _repository.DeleteClient(client);
                return new ServerResponse<string>(StatusCode.Ok, client.IdentityId);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
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


        // FOR USERS





        public async Task<ServerResponse<ClientFullInfoForUsers>> GetFullClientInfoForUsers(int id)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(id);
            }
            catch
            {
                return new ServerResponse<ClientFullInfoForUsers>(StatusCode.DbConnectionError, null);
            }

            if (client is null)
            {
                return null;
            }

            var clientTypes = new List<ParameterInfoForUsers>();
            foreach (var item in client.ClientClientTypes)
            {
                clientTypes.Add(new ParameterInfoForUsers()
                {
                    Id = item.ClientTypeId,
                    Title = item.ClientType.Title
                });
            }

            var mealTypes = new List<ParameterInfoForUsers>();
            foreach (var item in client.ClientMealTypes)
            {
                mealTypes.Add(new ParameterInfoForUsers()
                {
                    Id = item.MealTypeId,
                    Title = item.MealType.Title
                });
            }

            var cuisines = new List<ParameterInfoForUsers>();
            foreach (var item in client.ClientCuisines)
            {
                cuisines.Add(new ParameterInfoForUsers()
                {
                    Id = item.CuisineId,
                    Title = item.Cuisine.Title
                });
            }

            var features = new List<ParameterInfoForUsers>();
            foreach (var item in client.ClientFeatures)
            {
                features.Add(new ParameterInfoForUsers()
                {
                    Id = item.FeatureId,
                    Title = item.Feature.Title
                });
            }

            var photos = new List<string>();
            foreach (var item in client.ClientImages)
            {
                if (!item.IsMain)
                {
                    photos.Add(item.ImagePath);
                }
            }


            return new ServerResponse<ClientFullInfoForUsers>(StatusCode.Ok, new ClientFullInfoForUsers()
            {
                Id = client.Id,
                Lat = client.Lat,
                Long = client.Long,
                Description = client.Description,
                Name = client.RestaurantName,
                OpenTime = client.OpenTime,
                CloseTime = client.CloseTime,
                MainImage = client.ClientImages.FirstOrDefault(item => item.IsMain) is null ? null : client.ClientImages.FirstOrDefault(item => item.IsMain).ImagePath,
                LogoPath = client.LogoPath,
                ClientTypes = clientTypes,
                MealTypes = mealTypes,
                Cuisines = cuisines,
                Features = features,
                Phones = this.PhonesToList(client.ClientPhones),
                SocialLinks = this.SocialLinksToList(client.SocialLinks),
                Photos = photos
            });
        }


        public async Task<ServerResponse<ICollection<int>>> GetFavouritesIds(string userIdentityId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(userIdentityId);
                if (user is null)
                {
                    return new ServerResponse<ICollection<int>>(StatusCode.UserNotFound, null);
                }
                var response = new List<int>();
                foreach (var item in user.ClientFavourites)
                {
                    response.Add(item.ClientId);
                }
                return new ServerResponse<ICollection<int>>(StatusCode.Ok, response);
            }
            catch
            {
                return new ServerResponse<ICollection<int>>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse> AddFavourite(int clientId, string identityUserId)
        {
            User user;
            Client client;
            try
            {

                user = await _repository.GetUser(identityUserId);
                client = await _repository.GetClient(clientId);
                if (user is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
                if (client is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
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


        public async Task<ServerResponse<ClientShortInfoForUsersResponse>> GetComingSoon(int skip, int take)
        {
            IEnumerable<Client> clients;
            int count;
            try
            {
                clients = await _repository.GetComingSoon(skip, take);
                count = await _repository.GetComingSoonCount();
            }
            catch
            {
                return new ServerResponse<ClientShortInfoForUsersResponse>(StatusCode.DbConnectionError, null);
            }

            var response = new List<ClientShortInfoForUsers>();
            foreach (var item in clients)
            {
                response.Add(await this.ClientToClientShortInfoForUsers(item));
            }
            return new ServerResponse<ClientShortInfoForUsersResponse>(StatusCode.Ok,
                new ClientShortInfoForUsersResponse()
                {
                    TotalCount = count,
                    Clients = response
                });
        }



        private async Task<ClientShortInfoForUsers> ClientToClientShortInfoForUsers(Client client)
        {
            if (client is null)
            {
                return null;
            }

            string mainImage = null;

            if (client.ClientImages.Count > 0)
            {
                mainImage = client.ClientImages.FirstOrDefault(item => item.IsMain) is null ? client.ClientImages.First().ImagePath : client.ClientImages.FirstOrDefault(item => item.IsMain).ImagePath;
            }

            ICollection<Table> tables;
            try
            {
                tables = await _repository.GetClientTables(client.Id);
            }
            catch
            {
                return null;
            }

            var clientTypes = new List<ParameterInfoForUsers>();
            foreach (var item in client.ClientClientTypes)
            {
                clientTypes.Add(new ParameterInfoForUsers()
                {
                    Id = item.ClientTypeId,
                    Title = item.ClientType.Title
                });
            }

            var mealTypes = new List<ParameterInfoForUsers>();
            foreach (var item in client.ClientMealTypes)
            {
                mealTypes.Add(new ParameterInfoForUsers()
                {
                    Id = item.MealTypeId,
                    Title = item.MealType.Title
                });
            }

            return new ClientShortInfoForUsers()
            {
                Id = client.Id,
                Name = client.RestaurantName,
                Lat = client.Lat,
                Long = client.Long,
                MainImage = mainImage,
                LogoPath = client.LogoPath,
                TableTotalCount = tables.Count,
                MealTypes = mealTypes,
                ClientTypes = clientTypes,
                PriceCategory = client.PriceCategory,
                TableAvailableCount = 0 // change
            };
        }




        // ===================================================================================================





        // change
        public async Task<ServerResponse<ClientShortInfoForUsersResponse>> GetClientsByFilterForUsers(ClientFilter filter, int skip, int take)
        {
            return new ServerResponse<ClientShortInfoForUsersResponse>(StatusCode.Ok,
                new ClientShortInfoForUsersResponse()
                {
                    TotalCount = 0,
                    Clients = new List<ClientShortInfoForUsers>()
                });
        }




        public async Task<ICollection<ClientFullInfoForUsers>> GetFullClientInfoForUsers()
        {
            var clients = await _repository.GetClients();
            if (clients is null)
            {
                return null;
            }
            var res = new List<ClientFullInfoForUsers>();
            foreach (var item in clients)
            {
                res.Add(this.ClientToFullInfoForUsers(item));
            }
            return res;
        }

        public async Task<ICollection<ClientShortInfoForUsers>> GetFavourites(string identityUserId)
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
            var res = new List<ClientShortInfoForUsers>();
            foreach (var fav in favourites)
            {
                var galleryImages = fav.Client.ClientImages;

                //change
                res.Add(new ClientShortInfoForUsers()
                {
                    //Id = fav.Client.Id,
                    //MainImage = galleryImages.FirstOrDefault(img => img.IsMain) is null ? "" : galleryImages.FirstOrDefault(img => img.IsMain).ImagePath,
                    //Name = fav.Client.RestaurantName,
                });
            }
            return res;
        }





        public async Task<ICollection<ClientFullInfoForUsers>> GetClientsByMeal(string mealType)
        {
            var clients = await _repository.GetClientsByMeal(mealType);
            if (clients != null)
            {
                var res = new List<ClientFullInfoForUsers>();
                foreach (var client in clients)
                {
                    res.Add(this.ClientToFullInfoForUsers(client));
                }
                return res;
            }
            return null;
        }

        public async Task<IEnumerable<ClientFullInfoForUsers>> GetClientsByName(string title)
        {
            var clients = await _repository.GetClientsByName(title);
            if (clients != null)
            {
                var res = new List<ClientFullInfoForUsers>();
                foreach (var client in clients)
                {
                    res.Add(this.ClientToFullInfoForUsers(client));
                }
                return res;
            }
            return null;

        }



        public async Task<ClientHallsInfo> GetClientHalls(int id)
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
            return new ClientHallsInfo()
            {
                ClientId = id,
                Floors = floorsInfo
            };
        }




















        private ClientFullInfoForUsers ClientToFullInfoForUsers(Client client)
        {
            if (client is null)
            {
                return null;
            }
            return new ClientFullInfoForUsers()
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
                    IsWhatsApp = item.IsWhatsApp,
                    IsTelegram = item.IsTelegram
                });
            }
            return res;
        }

        private ICollection<string> ImagesToList(ICollection<ClientGalleryImage> images)
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
                    ImgPath = item.ImagePath is null ? item.Client.LogoPath : item.ImagePath,
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
