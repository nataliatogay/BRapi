using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using Microsoft.AspNetCore.Identity;

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
            ClientRequest clientRequest = await _repository.GetClientRequest(newClientRequest.ClientRequestId);
            Client client = new Client()
            {
                Name = newClientRequest.Name,
                Address = newClientRequest.Address,
                Lat = newClientRequest.Lat,
                Long = newClientRequest.Long,
                OpenTime = newClientRequest.OpenTime,
                CloseTime = newClientRequest.CloseTime,
                IsPasking = newClientRequest.IsPasking,
                IsWiFi = newClientRequest.IsWiFi,
                IsLiveMusic = newClientRequest.IsLiveMusic,
                IsOpenSpace = newClientRequest.IsOpenSpace,
                IsChildrenZone = newClientRequest.IsChildrenZone,
                IsBusinessLunch = newClientRequest.IsBusinessLunch,
                AdditionalInfo = newClientRequest.AdditionalInfo,
                MainImagePath = newClientRequest.MainImage,
                MaxReserveDays = newClientRequest.MaxReserveDays,
                ClientRequestId = newClientRequest.ClientRequestId,
                IsBlocked = false,
                IdentityId = identityId
            };

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


            clientRequest.ClientId = addedClient.Id;
            await _repository.UpdateClientRequest(clientRequest);
            
        }


        public async Task<bool> DeleteClient(int id)
        {
            var client = await _repository.GetClientById(id);
            if (client != null)
            {
                var clientRequest = await _repository.GetClientRequest(client.ClientRequestId);
                clientRequest.ClientId = null;
                await _repository.UpdateClientRequest(clientRequest);
                return await _repository.DeleteClient(client);
            }
            return false;
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await _repository.GetClients();
        }        

        public async Task<Client> GetClient(int id)
        {
            return await _repository.GetClientById(id);
        }

        public async Task<Client> UpdateClient(Client client)
        {
            var clientToUpdate = await _repository.GetClientById(client.Id);
            if(clientToUpdate is null)
            {
                return null;
            }
            clientToUpdate.Name = client.Name ?? clientToUpdate.Name;
            clientToUpdate.Address = client.Address ?? clientToUpdate.Address;
            clientToUpdate.OpenTime = client.OpenTime;
            clientToUpdate.CloseTime = client.CloseTime;
            clientToUpdate.AdditionalInfo = client.AdditionalInfo ?? clientToUpdate.AdditionalInfo;
            clientToUpdate.IsLiveMusic = client.IsLiveMusic;
            clientToUpdate.IsOpenSpace = client.IsOpenSpace;
            clientToUpdate.IsPasking = client.IsPasking;
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
    }
}
