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
                MainImagePath = newClientRequest.MainImage,
                IsBlocked = false,
                IdentityId = identityId
            };

            //client.MainImagePath = await _blobService.UploadImage(newClientRequest.MainImage);

            Client addedClient = await _repository.AddClient(client);

            foreach (var paymentTypeId in newClientRequest.PaymentTypeIds)
            {
                await _repository.AddClientPaymentType(addedClient.Id, paymentTypeId);
            }

            foreach (var mealTypeId in newClientRequest.MealTypeIds)
            {
                await _repository.AddClientMealType(addedClient.Id, mealTypeId);
            }

            foreach (var clientTypeId in newClientRequest.ClientTypeIds)
            {
                await _repository.AddClientClientType(addedClient.Id, clientTypeId);
            }

            foreach (var cuisineId in newClientRequest.CuisineIds)
            {
                await _repository.AddClientCuisine(addedClient.Id, cuisineId);
            }

            foreach (var link in newClientRequest.SocialLinks)
            {
                await _repository.AddClientSocialLink(addedClient.Id, link);
            }

            foreach (var phone in newClientRequest.Phones)
            {
                await _repository.AddClientPhone(
                    addedClient.Id,
                    phone.Number,
                    phone.IsShow
                    );
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

        public async Task<IEnumerable<ClientInfoResponse>> GetAllClients(string role)
        {
            var clients = await _repository.GetClients();
            if (clients != null)
            {
                var res = new List<ClientInfoResponse>();
                foreach (var client in clients)
                {
                    res.Add(this.ClientToClientInfoRequest(client, role));
                }
                return res;
            }
            return null;
        }

        public async Task<IEnumerable<ClientInfoResponse>> GetClientsByMeal(string mealType, string role)
        {
            var clients = await _repository.GetClientsByMeal(mealType);
            if (clients != null)
            {
                var res = new List<ClientInfoResponse>();
                foreach (var client in clients)
                {
                    res.Add(this.ClientToClientInfoRequest(client, role));
                }
                return res;
            }
            return null;
        }

        public async Task<IEnumerable<ClientInfoResponse>> GetClientsByName(string title, string role)
        {
            var clients = await _repository.GetClientsByName(title);
            if (clients != null)
            {
                var res = new List<ClientInfoResponse>();
                foreach (var client in clients)
                {
                    res.Add(this.ClientToClientInfoRequest(client, role));
                }
                return res;
            }
            return null;

        }

        public async Task<ClientInfoResponse> GetClient(int id, string role)
        {
            var client = await _repository.GetClient(id);
            if (client != null)
                return this.ClientToClientInfoRequest(client, role);
            return null;
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


        private ClientInfoResponse ClientToClientInfoRequest(Client client, string role)
        {
            // for users and clients
            var clientTypes = new List<string>();
            if (client.ClientClientTypes != null)
            {
                foreach (var type in client.ClientClientTypes)
                {
                    clientTypes.Add(type.ClientType.Title);
                }
            }
            var cuisins = new List<string>();

            if (client.ClientCuisines != null)
            {
                foreach (var cuisine in client.ClientCuisines)
                {
                    cuisins.Add(cuisine.Cuisine.Title);
                }
            }

            var meals = new List<string>();
            if (client.ClientMealTypes != null)
            {
                foreach (var meal in client.ClientMealTypes)
                {
                    meals.Add(meal.MealType.Title);
                }
            }

            var socials = new List<string>();
            if (client.SocialLinks != null)
            {
                foreach (var social in client.SocialLinks)
                {
                    socials.Add(social.Link);
                }
            }

            var paymentTypes = new List<string>();
            if (client.ClientPaymentTypes != null)
            {
                foreach (var type in client.ClientPaymentTypes)
                {
                    paymentTypes.Add(type.PaymentType.Title);
                }
            }

            var phones = new List<string>();
            if (client.ClientPhones != null)
            {
                foreach (var phone in client.ClientPhones)
                {
                    if (!phone.IsShow)
                    {
                        if (role.Equals("Admin"))
                        {
                            phones.Add(phone.Number);
                        }
                    }
                    else
                    {
                        phones.Add(phone.Number);
                    }
                }
            }
            var photos = new List<string>();
            if (client.ClientImages != null)
            {
                foreach (var photo in client.ClientImages)
                {
                    photos.Add(photo.ImagePath);
                }
            }

            var events = new List<EventInfo>();
            if (client.Events != null)
            {
                foreach (var item in client.Events)
                {
                    events.Add(new EventInfo()
                    {
                        Id = item.Id,
                        Title = item.Title,
                        Description = item.Description,
                        Date = item.Date,
                        ImgPath = item.ImagePath
                    });
                }
            }

            ClientInfoResponse res;
            if (role.Equals("Admin"))
            {
                res = new ClientInfoForAdminResponse()
                {
                    Id = client.Id,
                    Name = client.Name,
                    Address = client.Address,
                    OpenTime = client.OpenTime,
                    CloseTime = client.CloseTime,
                    IsBusinessLunch = client.IsBusinessLunch,
                    IsChildrenZone = client.IsChildrenZone,
                    IsWiFi = client.IsWiFi,
                    IsLiveMusic = client.IsLiveMusic,
                    IsOpenSpace = client.IsOpenSpace,
                    IsParking = client.IsParking,
                    AdditionalInfo = client.AdditionalInfo,
                    Lat = client.Lat,
                    Long = client.Long,
                    MaxReserveDays = client.MaxReserveDays,
                    SocialLinks = socials,
                    ClientTypes = clientTypes,
                    Cuisines = cuisins,
                    MealTypes = meals,
                    PaymentTypes = paymentTypes,
                    Phones = phones,
                    MainImage = client.MainImagePath,
                    Events = events,
                    Photos = photos,
                    Email = client.Identity.Email,
                    IsBlocked = client.IsBlocked
                };
            }
            else
            {
                res = new ClientInfoForUsersResponse()
                {
                    Id = client.Id,
                    Name = client.Name,
                    Address = client.Address,
                    OpenTime = client.OpenTime,
                    CloseTime = client.CloseTime,
                    IsBusinessLunch = client.IsBusinessLunch,
                    IsChildrenZone = client.IsChildrenZone,
                    IsWiFi = client.IsWiFi,
                    IsLiveMusic = client.IsLiveMusic,
                    IsOpenSpace = client.IsOpenSpace,
                    IsParking = client.IsParking,
                    AdditionalInfo = client.AdditionalInfo,
                    Lat = client.Lat,
                    Long = client.Long,
                    MaxReserveDays = client.MaxReserveDays,
                    SocialLinks = socials,
                    ClientTypes = clientTypes,
                    Cuisines = cuisins,
                    MealTypes = meals,
                    PaymentTypes = paymentTypes,
                    Phones = phones,
                    MainImage = client.MainImagePath,
                    Events = events,
                    Photos = photos
                };
            }
            return res;
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


    }
}
