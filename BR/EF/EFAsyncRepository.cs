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

        public async Task<Client> GeClientByIdentityId(string identityId)
        {
            return await _db.Clients.FirstOrDefaultAsync(c => c.IdentityId == identityId);
        }

        public async Task<IEnumerable<ToBeClient>> GetToBeClients()
        {
            return await _db.ToBeClients.Where(c => c.ClientId == null).ToListAsync();
        }

        public async Task<ToBeClient> GetToBeClient(int id)
        {
            return await _db.ToBeClients
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task UpdateToBeClient(ToBeClient toBeClient)
        {
            _db.ToBeClients.Update(toBeClient);
            await _db.SaveChangesAsync();
        }

        public async Task AddToBeClient(ToBeClient toBeClient)
        {
            await _db.ToBeClients.AddAsync(toBeClient);
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
    }
}




/*

public async Task<Admin> GetAdminByEmail(string email)
{
    return await _db.Admins.FirstOrDefaultAsync(a => a.Email == email);
}

public async Task<Client> GetClientByEmail(string email)
{
    return await _db.Clients
        .FirstOrDefaultAsync(c => c.Email == email);
}

public async Task<AccountToken> GetAdminToken(string refreshToken)
{
    return await _db.AdminAccountTokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
}

public async Task<AccountToken> GetAdminToken(int adminId)
{
    return await _db.AdminAccountTokens.FirstOrDefaultAsync(t => t.AdminId == adminId);
}

public async Task AddAdminToken(AccountToken adminRefreshToken)
{
    await _db.AdminAccountTokens.AddAsync(adminRefreshToken);
    await _db.SaveChangesAsync();
}

public async Task RemoveAdminToken(AccountToken adminRefreshToken)
{
    _db.AdminAccountTokens.Remove(adminRefreshToken);
    await _db.SaveChangesAsync();
}

public async Task AddClientToken(ClientAccountToken clientRefreshToken)
{
    await _db.ClientAccountTokens.AddAsync(clientRefreshToken);
    await _db.SaveChangesAsync();
}

public async Task<ClientAccountToken> GetClientToken(string refreshToken)
{
    return await _db.ClientAccountTokens.FirstOrDefaultAsync(t => t.RefreshToken == refreshToken);
}

public async Task RemoveClientToken(ClientAccountToken clientRefreshToken)
{
    _db.ClientAccountTokens.Remove(clientRefreshToken);
    await _db.SaveChangesAsync();
}
*/
