using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BR.Services
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly AuthOptions _authOptions;

        public ClientAccountService(IAsyncRepository repository, IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authOptions = options.Value;
        }

        public async Task<LogInResponse> LogIn(IdentityUser identityUser)
        {
            return await Authentication(identityUser);
        }

        public async Task LogOut(string refreshToken)
        {
            AccountToken accountToken = await _repository.GetToken(refreshToken);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }

        public async Task<Client> GetInfo(string identityId)
        {
            return await _repository.GetClientByIdentityId(identityId);
        }

        public async Task<LogInResponse> UpdateToken(string refreshToken)
        {
            AccountToken token = await _repository.GetToken(refreshToken);
            if (token is null)
            {
                return null;
            }
            if (token.Expires <= DateTime.Now)
            {
                return null;
            }
            IdentityUser identityUser = await _repository.GetIdentityUser(token.IdentityUserId);
           // Client client = await _repository.GetClientById(token.ClientId);

            if (identityUser is null)
            {
                return null;
            }
            return await Authentication(identityUser);
        }

        public async Task<bool> ClientIsBlocked(string identityId)
        {
            var client = await _repository.GetClientByIdentityId(identityId);
            if (client != null && client.IsBlocked)
            {
                return true;
            }
            return false;
        }

        private async Task<LogInResponse> Authentication(IdentityUser identityUser)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, identityUser.UserName)
            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims, "Token", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            JwtSecurityToken token = new JwtSecurityToken(
                issuer: _authOptions.Issuer,
                audience: _authOptions.Audience,
                claims: claimsIdentity.Claims,
                expires: DateTime.Now.AddMinutes(_authOptions.AccessLifetime),
                signingCredentials: new SigningCredentials(
                        _authOptions.GetSymmetricSecurityKey(),
                        SecurityAlgorithms.HmacSha256)
                );
            string tokenStr = new JwtSecurityTokenHandler().WriteToken(token);

            LogInResponse resp = new LogInResponse()
            {
                AccessToken = tokenStr,
                RefreshToken = Guid.NewGuid().ToString()
            };

            await _repository.AddToken(new AccountToken()
            {
                IdentityUserId = identityUser.Id,
                RefreshToken = resp.RefreshToken,
                Expires = DateTime.Now.AddMinutes(_authOptions.RefreshLifetime)
            });
            return resp;
        }
    }
}
