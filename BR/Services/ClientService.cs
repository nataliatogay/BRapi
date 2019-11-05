using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;

namespace BR.Services
{
    
    public class ClientService : IClientService
    {
        private readonly IAsyncRepository _repository;
        private readonly IEmailService _emailService;

        public ClientService(IAsyncRepository repository,
            IEmailService emailService)
        {
            _repository = repository;
            _emailService = emailService;
        }

        /*
        public async Task AddNewClient(Client client)
        {
            ToBeClient toBeClient = await _repository.GetToBeClient(client.ToBeClientId);
            Client addedClient = await _repository.AddClient(client);
            toBeClient.ClientId = addedClient.Id;
            await _repository.UpdateToBeClient(toBeClient);
            try
            {
                string msgBody = $"Login: {client.Email}\nPassword: {client.Password}";
                var sendMail = new SendMailRequest()
                {
                    ToAddress = client.Email,
                    Subject = "Registration info",
                    Body = msgBody
                };
                await _emailService.SendAsync(sendMail);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        */

        public async Task<bool> DeleteClient(int id)
        {
            var client = await _repository.GetClientById(id);
            if (client != null)
            {
                await _repository.DeleteClient(client);
                return true;
            }
            return false;
        }

        public async Task<IEnumerable<Client>> GetAllClients()
        {
            return await _repository.GetClients();
        }

        public async Task AddNewToBeClient(ToBeClient toBeClient)
        {
            await _repository.AddToBeClient(toBeClient);
        }

        public async Task<IEnumerable<ToBeClient>> GetAllToBeClients()
        {
            return await _repository.GetToBeClients();
        }

        public async Task<Client> GetClient(int id)
        {
            return await _repository.GetClientById(id);
        }

        public async Task<ToBeClient> GetToBeClient(int id)
        {
            return await _repository.GetToBeClient(id);
        }

        public async Task<int> ToBeClientCount()
        {
            var res = await _repository.GetToBeClients();
            if (res is null)
            {
                return 0;
            }
            return res.Count();
        }

        public async Task<Client> UpdateClient(Client client)
        {
            var clientToUpdate = await _repository.GetClientById(client.Id);
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
