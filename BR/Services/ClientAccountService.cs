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

        public async Task<LogInResponse> LogIn(string email, string password)
        {
            Client client = await _repository.GetClientByEmail(email);

            if (client is null)
            {
                return null;
            }
            return await Authentication(client);
        }

        public async Task LogOut(string refreshToken)
        {
            ClientAccountToken accountToken = await _repository.GetClientToken(refreshToken);
            if (!(accountToken is null))
            {
                await _repository.RemoveClientToken(accountToken);
            }
        }

        public async Task<Client> GetInfo(int id)
        {
            return await _repository.GetClientById(id);
        }

        public async Task<LogInResponse> UpdateToken(string refreshToken)
        {
            ClientAccountToken token = await _repository.GetClientToken(refreshToken);
            if (token is null)
            {
                return null;
            }
            if (token.Expires <= DateTime.Now)
            {
                return null;
            }
            Client client = await _repository.GetClientById(token.ClientId);

            if (client is null)
            {
                return null;
            }
            return await Authentication(client);
        }

        private async Task<LogInResponse> Authentication(Client client)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, client.Email),
                new Claim("id", client.Id.ToString())
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
                Email = client.Email,
                AccessToken = tokenStr,
                RefreshToken = Guid.NewGuid().ToString()
            };

            await _repository.AddClientToken(new ClientAccountToken()
            {
                ClientId = client.Id,
                RefreshToken = resp.RefreshToken,
                Expires = DateTime.Now.AddMinutes(_authOptions.RefreshLifetime)
            });


            return resp;
        }
    }
}
