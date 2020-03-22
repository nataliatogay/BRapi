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

namespace BR.Services
{

    public class ClientService : IClientService
    {
        private readonly IAsyncRepository _repository;
        //private readonly IEmailService _emailService;

        public ClientService(IAsyncRepository repository)
        {
            _repository = repository;
        }

        public async Task AddNewClient(NewClientRequest newClientRequest, string identityId)
        {
            Client client = new Client()
            {
                Name = newClientRequest.Name,
                Address = newClientRequest.Address,
                Lat = newClientRequest.Lat,
                Long = newClientRequest.Long,
                OpenTime = newClientRequest.OpenTime,
                CloseTime = newClientRequest.CloseTime,
                IsParking = newClientRequest.IsParking,
                IsWiFi = newClientRequest.IsWiFi,
                IsLiveMusic = newClientRequest.IsLiveMusic,
                IsOpenSpace = newClientRequest.IsOpenSpace,
                IsChildrenZone = newClientRequest.IsChildrenZone,
                IsBusinessLunch = newClientRequest.IsBusinessLunch,
                AdditionalInfo = newClientRequest.AdditionalInfo,
                MaxReserveDays = newClientRequest.MaxReserveDays,
                ReserveDurationAvg = newClientRequest.ReserveDurationAvg,
                ConfirmationDuration = newClientRequest.ConfirmationDuration,
                MainImagePath = newClientRequest.MainImage,
                IsBlocked = false,
                IdentityId = identityId,
                RegistrationDate = DateTime.Now
            };
            //if (client.MainImagePath is null)
            //{
            //    client.MainImagePath = "https://rb2020storage.blob.core.windows.net/photos/default_restaurant.jpg";
            //}

            //client.MainImagePath = await _blobService.UploadImage(newClientRequest.MainImage);

            Client addedClient = await _repository.AddClient(client);

            if (newClientRequest.PaymentTypeIds != null)
            {
                foreach (var paymentTypeId in newClientRequest.PaymentTypeIds)
                {
                    await _repository.AddClientPaymentType(new ClientPaymentType()
                    {
                        ClientId = addedClient.Id,
                        PaymentTypeId = paymentTypeId
                    });
                }
            }

            if (newClientRequest.MealTypeIds != null)
            {
                foreach (var mealTypeId in newClientRequest.MealTypeIds)
                {
                    await _repository.AddClientMealType(
                        new ClientMealType()
                        {
                            ClientId = addedClient.Id,
                            MealTypeId = mealTypeId
                        });
                }
            }

            if (newClientRequest.ClientTypeIds != null)
            {
                foreach (var clientTypeId in newClientRequest.ClientTypeIds)
                {
                    await _repository.AddClientClientType(new ClientClientType()
                    {
                        ClientId = addedClient.Id,
                        ClientTypeId = clientTypeId
                    });
                }
            }

            if (newClientRequest.CuisineIds != null)
            {
                foreach (var cuisineId in newClientRequest.CuisineIds)
                {
                    await _repository.AddClientCuisine(new ClientCuisine()
                    {
                        ClientId = addedClient.Id,
                        CuisineId = cuisineId
                    });
                }
            }

            if (newClientRequest.SocialLinks != null)
            {
                foreach (var link in newClientRequest.SocialLinks)
                {
                    await _repository.AddClientSocialLink(new SocialLink()
                    {
                        ClientId = addedClient.Id,
                        Link = link
                    });
                }
            }

            if (newClientRequest.Phones != null)
            {
                foreach (var phone in newClientRequest.Phones)
                {
                    await _repository.AddClientPhone(
                        new ClientPhone()
                        {
                            ClientId = addedClient.Id,
                            Number = phone.Number,
                            IsWhatsApp = phone.IsWhatsApp
                        });
                }
            }

            if (newClientRequest.ClientRequestId != null)
            {
                ClientRequest clientRequest = await _repository.GetClientRequest(newClientRequest.ClientRequestId ?? default(int));
                if (clientRequest != null)
                {
                    clientRequest.ClientId = addedClient.Id;
                }
                await _repository.UpdateClientRequest(clientRequest);
            }
        }

        public async Task<bool> DeleteClient(int id)
        {
            var client = await _repository.GetClient(id);
            if (client != null)
            {
                //var clientRequest = await _repository.GetClientRequest(client.ClientRequestId);
                //clientRequest.ClientId = null;
                // await _repository.UpdateClientRequest(clientRequest);
                return await _repository.DeleteClient(client);
            }
            return false;
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
                    Name = client.Name,
                    MainImage = client.MainImagePath,
                    RegistrationDate = client.RegistrationDate
                });
            }
            return res;

        }

        public async Task<ICollection<ClientShortInfoForAdminResponse>> GetShortClientInfoForAdmin()
        {
            var clients = await _repository.GetClients();
            if (clients is null)
            {
                return null;
            }
            var res = new List<ClientShortInfoForAdminResponse>();
            foreach (var client in clients)
            {
                res.Add(new ClientShortInfoForAdminResponse()
                {
                    Id = client.Id,
                    Name = client.Name,
                    MainImage = client.MainImagePath,
                    Address = client.Address,
                    Email = client.Identity.Email,
                    RegistrationDate = client.RegistrationDate,
                    IsBlocked = client.IsBlocked
                });
            }
            return res;

        }

        public async Task<ClientFullInfoForAdminResponse> GetFullClientInfoForAdmin(int id)
        {
            var client = await _repository.GetClient(id);
            if (client is null)
            {
                return null;
            }
            return new ClientFullInfoForAdminResponse()
            {
                Id = client.Id,
                Name = client.Name,
                MainImage = client.MainImagePath,
                Address = client.Address,
                Email = client.Identity.Email,
                RegistrationDate = client.RegistrationDate,
                IsBlocked = client.IsBlocked,
                OpenTime = client.OpenTime,
                CloseTime = client.CloseTime,
                AdditionalInfo = client.AdditionalInfo,
                IsBusinessLunch = client.IsBusinessLunch,
                IsChildrenZone = client.IsChildrenZone,
                IsLiveMusic = client.IsLiveMusic,
                IsOpenSpace = client.IsOpenSpace,
                IsParking = client.IsParking,
                IsWiFi = client.IsWiFi,
                Lat = client.Lat,
                Long = client.Long,
                MaxReserveDays = client.MaxReserveDays,
                IsBarReservation = client.BarReserveDuration is null ? false : true,
                ReserveDurationAvg = client.ReserveDurationAvg,
                ConfirmationDuration = client.ConfirmationDuration,
                ClientTypes = this.ClientTypesToList(client.ClientClientTypes),
                Cuisines = this.CuisinesToList(client.ClientCuisines),
                PaymentTypes = this.PaymentTypesToList(client.ClientPaymentTypes),
                Events = this.EventsToList(client.Events),
                Phones = this.PhonesToList(client.ClientPhones),
                SocialLinks = this.SocialLinksToList(client.SocialLinks),
                MealTypes = this.MealTypesToList(client.ClientMealTypes),
                Photos = this.ImagesToList(client.ClientImages)
            };
        }

        public async Task<ClientFullInfoForUsersResponse> GetFullClientInfoForUsers(int id)
        {
            var client = await _repository.GetClient(id);
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
            var favourites = await _repository.GetFavourites(user.Id);
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
                    Name = fav.Client.Name,
                    RegistrationDate = fav.Client.RegistrationDate
                });
            }
            return res;
        }

        public async Task<bool> AddFavourite(int clientId, string identityUserId)
        {
            var user = await _repository.GetUser(identityUserId);
            if (user is null)
            {
                return false;
            }

            var favourites = await _repository.GetFavourites(user.Id);
            var clientFav = new ClientFavourite()
            {
                ClientId = clientId,
                UserId = user.Id
            };
            if (favourites.Contains(clientFav))
            {
                return false;
            }
            try
            {
                var res = await _repository.AddFavourite(clientFav);
                if (res is null)
                {
                    return false;
                }
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // return concurrencyError;
                return false;
            }
            catch (DbUpdateException ex)
            {
                // return UpdateError
                return false;
            }

        }

        public async Task<bool> DeleteFavourite(int clientId, string identityUserId)
        {
            var user = await _repository.GetUser(identityUserId);
            if (user is null)
            {
                return false;
            }

            var favourite = await _repository.GetFavourite(clientId, user.Id);
            if (favourite is null)
            {
                return false;
            }

            return await _repository.DeleteFavourite(favourite);
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
                Id = client.Id,
                Name = client.Name,
                MainImage = client.MainImagePath,
                Address = client.Address,
                Email = client.Identity.Email,
                RegistrationDate = client.RegistrationDate,
                IsBlocked = client.IsBlocked,
                OpenTime = client.OpenTime,
                CloseTime = client.CloseTime,
                AdditionalInfo = client.AdditionalInfo,
                IsBusinessLunch = client.IsBusinessLunch,
                IsChildrenZone = client.IsChildrenZone,
                IsLiveMusic = client.IsLiveMusic,
                IsOpenSpace = client.IsOpenSpace,
                IsParking = client.IsParking,
                IsWiFi = client.IsWiFi,
                Lat = client.Lat,
                Long = client.Long,
                MaxReserveDays = client.MaxReserveDays,
                IsBarReservation = client.BarReserveDuration is null ? false : true,
                ReserveDurationAvg = client.ReserveDurationAvg,
                ConfirmationDuration = client.ConfirmationDuration,
                ClientTypes = this.ClientTypesToList(client.ClientClientTypes),
                Cuisines = this.CuisinesToList(client.ClientCuisines),
                PaymentTypes = this.PaymentTypesToList(client.ClientPaymentTypes),
                Events = this.EventsToList(client.Events),
                Phones = this.PhonesToList(client.ClientPhones),
                SocialLinks = this.SocialLinksToList(client.SocialLinks),
                MealTypes = this.MealTypesToList(client.ClientMealTypes),
                Photos = this.ImagesToList(client.ClientImages)
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

        public async Task<Client> UpdateClient(Client client)
        {
            var clientToUpdate = await _repository.GetClient(client.Id);
            if (clientToUpdate is null)
            {
                return null;
            }
            clientToUpdate.Name = client.Name;
            clientToUpdate.Address = client.Address;
            clientToUpdate.OpenTime = client.OpenTime;
            clientToUpdate.CloseTime = client.CloseTime;
            clientToUpdate.AdditionalInfo = client.AdditionalInfo ?? clientToUpdate.AdditionalInfo;
            clientToUpdate.IsLiveMusic = client.IsLiveMusic;
            clientToUpdate.IsOpenSpace = client.IsOpenSpace;
            clientToUpdate.IsParking = client.IsParking;
            clientToUpdate.IsWiFi = client.IsWiFi;
            await _repository.UpdateClient(clientToUpdate);
            return clientToUpdate;
        }

        public string GeneratePassword()
        {
            Random random = new Random();
            string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 8; ++i)
            {
                password.Append(letters.ElementAt(random.Next(0, letters.Length)));
            }
            return password.ToString();
        }



        public async Task<Client> BlockClient(BlockUserRequest blockRequest)
        {
            var client = await _repository.GetClient(blockRequest.UserId);
            if (client is null)
            {
                return null;
            }
            client.IsBlocked = blockRequest.ToBlock;
            return await _repository.UpdateClient(client);
        }



        private ClientFullInfoForUsersResponse ClientToFullInfoForUsers(Client client)
        {
            if (client is null)
            {
                return null;
            }
            return new ClientFullInfoForUsersResponse()
            {
                Id = client.Id,
                Name = client.Name,
                Address = client.Address,
                Lat = client.Lat,
                Long = client.Long,
                OpenTime = client.OpenTime,
                CloseTime = client.CloseTime,
                IsBusinessLunch = client.IsBusinessLunch,
                IsChildrenZone = client.IsChildrenZone,
                IsLiveMusic = client.IsLiveMusic,
                IsOpenSpace = client.IsOpenSpace,
                IsParking = client.IsParking,
                IsWiFi = client.IsWiFi,
                MaxReserveDays = client.MaxReserveDays,
                IsBarReservation = client.BarReserveDuration is null ? false : true,
                ReserveDurationAvg = client.ReserveDurationAvg,
                ConfirmationDuration = client.ConfirmationDuration,
                MainImage = client.MainImagePath,
                ClientTypes = this.ClientTypesToList(client.ClientClientTypes),
                Cuisines = this.CuisinesToList(client.ClientCuisines),
                MealTypes = this.MealTypesToList(client.ClientMealTypes),
                PaymentTypes = this.PaymentTypesToList(client.ClientPaymentTypes),
                Photos = this.ImagesToList(client.ClientImages),
                Phones = this.PhonesToList(client.ClientPhones),
                SocialLinks = this.SocialLinksToList(client.SocialLinks),
                Events = this.EventsToList(client.Events)
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

        private ICollection<string> PaymentTypesToList(ICollection<ClientPaymentType> paymentTypes)
        {
            if (paymentTypes is null)
            {
                return null;
            }
            var res = new List<string>();
            foreach (var item in paymentTypes)
            {
                res.Add(item.PaymentType.Title);
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


    }
}
