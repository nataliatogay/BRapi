using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IAdminMailService
    {

        Task<string> GetUserEmail(int userId);
        Task<string> GetClientEmail(int clientId);
        Task<IEnumerable<UserMail>> GetAllUserMails(string adminIdentityName);
        Task<UserMail> GetUserMail(int userMailId);
        
        Task SendUserMail(string adminName, SendMailRequest sendMailRequest);
        Task DeleteUserMail(int userMailId);

        Task<IEnumerable<ClientMail>> GetAllClientMails(string adminIdentityName);
        Task<ClientMail> GetClientMail(int clientMailId);

        Task SendClientMail(string adminName, SendMailRequest sendMailRequestl);
        Task DeleteClientMail(int clientMailId);
    }
}
