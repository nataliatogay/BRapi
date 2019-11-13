using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BR.EF
{
    public class EFAsyncRepository : IAsyncRepository
    {
        private readonly BRDbContext _db;

        public EFAsyncRepository(BRDbContext db)
        {
            _db = db;
        }

        public async Task<IdentityUser> GetIdentityUser(string id)
        {
            return await _db.Users.FindAsync(id);
        }

        public async Task<Client> AddClient(Client client)
        {
            var res = await _db.Clients.AddAsync(client);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task DeleteClient(Client client)
        {
            _db.Clients.Remove(client);
            await _db.SaveChangesAsync();
        }

        public async Task<Client> GetClientById(int id)
        {
            return await _db.Clients
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<IEnumerable<Client>> GetClients()
        {
            return await _db.Clients.ToListAsync();
        }

        public async Task UpdateClient(Client client)
        {
            _db.Clients.Update(client);
            await _db.SaveChangesAsync();
        }

        public async Task<Admin> GetAdminByIdentityId(string identityId)
        {
            return await _db.Admins.FirstOrDefaultAsync(a => a.IdentityId == identityId);
        }

        public async Task<Admin> GetAdminByIdentityName(string identityName)
        {
            var identityUser = _db.Users.FirstOrDefault(u => u.UserName == identityName);
            if (identityUser != null)
            {
                return await this.GetAdminByIdentityId(identityUser.Id);
            }
            return null;

        }

        public async Task<Client> GetClientByIdentityId(string identityId)
        {
            return await _db.Clients.FirstOrDefaultAsync(c => c.IdentityId == identityId);
        }

        public async Task<IEnumerable<ClientRequest>> GetClientRequests()
        {
            return await _db.ClientRequests.Where(c => c.ClientId == null).ToListAsync();
        }

        public async Task<ClientRequest> GetClientRequest(int id)
        {
            return await _db.ClientRequests
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateClientRequest(ClientRequest clientRequest)
        {
            _db.ClientRequests.Update(clientRequest);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientRequest(ClientRequest clientRequest)
        {
            await _db.ClientRequests.AddAsync(clientRequest);
            await _db.SaveChangesAsync();
        }

        public async Task<Admin> AddAdmin(Admin admin)
        {
            var res = await _db.Admins.AddAsync(admin);
            await _db.SaveChangesAsync();
            return res.Entity;
        }


        public async Task<AccountToken> GetToken(string refreshToken)
        {
            return await _db.AccountTokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
        }

        public async Task AddToken(AccountToken refreshToken)
        {
            await _db.AccountTokens.AddAsync(refreshToken);
            await _db.SaveChangesAsync();
        }

        public async Task RemoveToken(AccountToken refreshToken)
        {
            _db.AccountTokens.Remove(refreshToken);
            await _db.SaveChangesAsync();
        }

        public async Task AddClientPaymentType(int clientId, int paymentTypeId)
        {
            await _db.ClientPaymentTypes.AddAsync(new ClientPaymentType()
            {
                ClientId = clientId,
                PaymentTypeId = paymentTypeId
            });
            await _db.SaveChangesAsync();

        }

        public async Task AddClientClientType(int clientId, int clientTypeId)
        {
            await _db.ClientClientTypes.AddAsync(new ClientClientType()
            {
                ClientId = clientId,
                ClientTypeId = clientTypeId
            });
            await _db.SaveChangesAsync();
        }

        public async Task AddClientCuisine(int clientId, int cuisineId)
        {
            await _db.ClientCuisines.AddAsync(new ClientCuisine()
            {
                ClientId = clientId,
                CuisineId = cuisineId
            });
            await _db.SaveChangesAsync();
        }

        public async Task AddClientSocialLink(int clientId, string link)
        {
            await _db.SocialLinks.AddAsync(new SocialLink()
            {
                ClientId = clientId,
                Link = link
            });
            await _db.SaveChangesAsync();
        }

        public async Task AddClientPhone(int clientId, int phoneCodeId, string phoneNumber)
        {
            await _db.ClientPhones.AddAsync(new ClientPhone()
            {
                ClientId = clientId,
                PhoneCodeId = phoneCodeId,
                PhoneNumber = phoneNumber
            });
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserMail>> GetAllUserMailsByAdminId(int adminId)
        {
            return await _db.UserMails.Where(m => m.AdminId == adminId).ToListAsync();
        }

        public async Task<UserMail> GetUserMail(int mailId)
        {
            return await _db.UserMails.FindAsync(mailId);
        }
        public async Task AddUserMail(UserMail userMail)
        {
            await _db.UserMails.AddAsync(userMail);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteUserMail(UserMail userMail)
        {
            _db.UserMails.Remove(userMail);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<ClientMail>> GetAllClientMailsByAdminId(int adminId)
        {
            return await _db.ClientMails.Where(m => m.AdminId == adminId).ToListAsync();
        }

        public async Task<ClientMail> GetClientMail(int mailId)
        {
            return await _db.ClientMails.FindAsync(mailId);
        }
        public async Task AddClientMail(ClientMail clientMail)
        {
            await _db.ClientMails.AddAsync(clientMail);
            await _db.SaveChangesAsync();
        }
        public async Task DeleteClientMail(ClientMail clientMail)
        {
            _db.ClientMails.Remove(clientMail);
            await _db.SaveChangesAsync();
        }

        public async Task<IEnumerable<PaymentType>> GetAllPaymentTypes()
        {
            return await _db.PaymentTypes.ToListAsync();
        }
        public async Task<IEnumerable<Cuisine>> GetAllCuisines()
        {
            return await _db.Cuisines.ToListAsync();
        }
        public async Task<IEnumerable<ClientType>> GetAllClientTypes()
        {
            return await _db.ClientTypes.ToListAsync();
        }

        public async Task<User> GetUser(int id)
        {
            return await _db.ApplicationUsers.FindAsync(id);
        }

        public async Task<User> AddUser(User user)
        {
            var res = await _db.ApplicationUsers.AddAsync(user);
            await _db.SaveChangesAsync();
            return res.Entity;
        }
    }
}


