using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Account;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BR.Services
{
    public class ClientAccountService : IClientAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly IBlobService _blobService;
        private readonly AuthOptions _authOptions;

        public ClientAccountService(IAsyncRepository repository,
            IBlobService blobService,
            IOptions<AuthOptions> options)
        {
            _repository = repository;
            _blobService = blobService;
            _authOptions = options.Value;
        }

        public async Task<LogInResponse> LogIn(IdentityUser identityUser, string notificationTag)
        {
            return await Authentication(identityUser, notificationTag);
        }


        public async Task LogOut(string notificationTag)
        {
            AccountToken accountToken = await _repository.GetTokenByTag(notificationTag);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }

        public async Task<Client> GetInfo(string identityId)
        {
            return await _repository.GetClient(identityId);
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
            return await Authentication(identityUser, token.NotificationTag);
        }

        public async Task<bool> ClientIsBlocked(string identityId)
        {
            var client = await _repository.GetClient(identityId);
            if (client != null && client.IsBlocked)
            {
                return true;
            }
            return false;
        }

        private async Task<LogInResponse> Authentication(IdentityUser identityUser, string notificationTag)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, identityUser.UserName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Client")
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

        public async Task<string> UploadMainImage(string identityId, string imageString)
        {
            var client = await _repository.GetClient(identityId);
            client.MainImagePath = await _blobService.UploadImage(imageString);
            await _repository.UpdateClient(client);
            return client.MainImagePath;
        }

        public async Task<ClientImage> UploadImage(string identityId, string imageString)
        {
            var client = await _repository.GetClient(identityId);
            var imgPath = await _blobService.UploadImage(imageString);
            var clientImage = new ClientImage()
            {
                ClientId = client.Id,
                ImagePath = imgPath
            };
            return await _repository.AddClientImage(clientImage);
        }

        public async Task<bool> DeleteImage(int id)
        {
            var img = await _repository.GetClientImage(id);
            if(img is null)
            {
                return false;
            }
            return await _blobService.DeleteImage(img.ImagePath);
        }
    }
}
