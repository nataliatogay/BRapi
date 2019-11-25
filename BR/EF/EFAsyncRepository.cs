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

        public async Task<bool> DeleteClient(Client client)
        {
            try
            {
                _db.Clients.Remove(client);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
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


        public async Task<IEnumerable<AccountToken>> GetTokens(string identityId)
        {
            return await _db.AccountTokens.Where(t => t.IdentityUserId == identityId).ToListAsync();
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

        public async Task AddClientMealType(int clientId, int mealtTypeId)
        {
            await _db.ClientMealTypes.AddAsync(new ClientMealType()
            {
                ClientId = clientId,
                MealTypeId = mealtTypeId
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

        public async Task AddClientPhone(int clientId, string phoneNumber, bool isShow)
        {
            var phone = new ClientPhone()
            {
                ClientId = clientId,
                // PhoneCodeId = phoneCodeId,
                Number = phoneNumber,
                IsShow = isShow

                //IsWhatsApp = isWhatsApp,
                //IsTelegram = isTelegram,
                //IsViber = isViber
            };
            await _db.ClientPhones.AddAsync(phone);

            // whatsapp 
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

        public async Task<Cuisine> AddCuisine(Cuisine cuisine)
        {
            var res = await _db.Cuisines.AddAsync(cuisine);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<Cuisine> GetCuisine(int id)
        {
            return await _db.Cuisines.FindAsync(id);
        }

        public async Task<Cuisine> GetCuisine(string title)
        {
            return await _db.Cuisines.FirstOrDefaultAsync(c => c.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<Cuisine> UpdateCuisine(Cuisine cuisine)
        {
            var res = _db.Cuisines.Update(cuisine);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeleteCuisine(Cuisine cuisine)
        {
            try
            {
                _db.Cuisines.Remove(cuisine);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<ClientType> AddClientType(ClientType clientType)
        {
            var res = await _db.ClientTypes.AddAsync(clientType);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<ClientType> GetClientType(int id)
        {
            return await _db.ClientTypes.FindAsync(id);
        }

        public async Task<ClientType> GetClientType(string title)
        {
            return await _db.ClientTypes.FirstOrDefaultAsync(t => t.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<ClientType> UpdateClientType(ClientType clientType)
        {
            var res = _db.ClientTypes.Update(clientType);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeleteClientType(ClientType clientType)
        {
            try
            {
                _db.ClientTypes.Remove(clientType);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<PaymentType> AddPaymentType(PaymentType paymentType)
        {
            var res = await _db.PaymentTypes.AddAsync(paymentType);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<PaymentType> GetPaymentType(int id)
        {
            return await _db.PaymentTypes.FindAsync(id);
        }

        public async Task<PaymentType> GetPaymentType(string title)
        {
            return await _db.PaymentTypes.FirstOrDefaultAsync(t => t.Title.ToUpper().Equals(title.ToUpper()));
        }

        public async Task<PaymentType> UpdatePaymentType(PaymentType paymentType)
        {
            var res = _db.PaymentTypes.Update(paymentType);
            await _db.SaveChangesAsync();
            return res.Entity;
        }

        public async Task<bool> DeletePaymentType(PaymentType paymentType)
        {
            try
            {
                _db.PaymentTypes.Remove(paymentType);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }


        public async Task<User> GetUser(int id)
        {
            return await _db.ApplicationUsers.FindAsync(id);
        }

        public async Task<IEnumerable<MealType>> GetAllMealTypes()
        {
            return await _db.MealTypes.ToListAsync();
        }

        public async Task<User> GetUser(string identityId)
        {
            return await _db.ApplicationUsers.FirstOrDefaultAsync(u => u.IdentityId == identityId);
        }

        public async Task<User> AddUser(User user)
        {
            var res = await _db.ApplicationUsers.AddAsync(user);
            await _db.SaveChangesAsync();
            return res.Entity;
        }









        public async Task<IEnumerable<Waiter>> GetWaitersByClientId(int clientId)
        {
            return await _db.Waiters.Where(w => w.ClientId == clientId).ToListAsync();
        }

        public async Task<Waiter> GetWaiter(int id)
        {
            return await _db.Waiters.FindAsync(id);
        }
        public async Task<Waiter> AddWaiter(Waiter waiter)
        {
            var res = _db.Waiters.Add(waiter);
            await _db.SaveChangesAsync();
            return res.Entity;
        }
        public async Task UpdateWaiter(Waiter waiter)
        {
            _db.Waiters.Update(waiter);
            await _db.SaveChangesAsync();
        }
        public async Task<bool> DeleteWaiter(Waiter waiter)
        {
            try
            {
                _db.Waiters.Remove(waiter);
                await _db.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }




    }
}

/*
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
 */


