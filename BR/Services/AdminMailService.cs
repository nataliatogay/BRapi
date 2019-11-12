using BR.DTO;
using BR.EF;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BR.Services
{
    public class AdminMailService : IAdminMailService
    {
        private readonly IAsyncRepository _repository;
        public AdminMailService(IAsyncRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GetUserEmail(int userId)
        {
            var user = await _repository.GetUser(userId);
            if(user != null)
            {

                var identityUser = await _repository.GetIdentityUser(user.IdentityId);
                if (identityUser != null)
                {
                    return identityUser.Email;
                }
            }
            return null;
        }

        public async Task<string> GetClientEmail(int clientId)
        {
            var client = await _repository.GetClientById(clientId);
            if (client != null)
            {
                //return client.Identity.Email;
                var identityUser = await _repository.GetIdentityUser(client.IdentityId);
                if (identityUser != null)
                {
                    return identityUser.Email;
                }
            }
            return null;
        }

        public async Task<IEnumerable<UserMail>> GetAllUserMails(string adminIdentityName)
        {
            var admin = await _repository.GetAdminByIdentityName(adminIdentityName);
            return await _repository.GetAllUserMailsByAdminId(admin.Id);
        }

        public async Task<UserMail> GetUserMail(int userMailId)
        {
            return await _repository.GetUserMail(userMailId);
        }

        public async Task SendUserMail(string adminName, SendMailRequest sendMailRequest)
        {
            Admin admin = await _repository.GetAdminByIdentityName(adminName);
            UserMail userMail = new UserMail()
            {
                AdminId = admin.Id,
                UserId = sendMailRequest.RecipentId,
                Subject = sendMailRequest.Subject,
                Body = sendMailRequest.Body,
                TimeSend = DateTime.Now
            };
            await _repository.AddUserMail(userMail);

        }

        public async Task DeleteUserMail(int userMailId)
        {
            var userMail = await _repository.GetUserMail(userMailId);
            await _repository.DeleteUserMail(userMail);
        }

        public async Task<IEnumerable<ClientMail>> GetAllClientMails(string adminIdentityName)
        {
            var admin = await _repository.GetAdminByIdentityName(adminIdentityName);
            return await _repository.GetAllClientMailsByAdminId(admin.Id);
        }

        public async Task<ClientMail> GetClientMail(int clientMailId)
        {
            return await _repository.GetClientMail(clientMailId);
        }
        public async Task SendClientMail(string adminName, SendMailRequest sendMailRequest)
        {
            Admin admin = await _repository.GetAdminByIdentityName(adminName);
            ClientMail clientMail = new ClientMail()
            {
                AdminId = admin.Id,
                ClientId = sendMailRequest.RecipentId,
                Subject = sendMailRequest.Subject,
                Body = sendMailRequest.Body,
                TimeSend = DateTime.Now
            };
            await _repository.AddClientMail(clientMail);
        }

        public async Task DeleteClientMail(int clientMailId)
        {
            var clientMail = await _repository.GetClientMail(clientMailId);
            await _repository.DeleteClientMail(clientMail);
        }

    }
}
