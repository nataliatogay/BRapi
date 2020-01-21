using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BR.Services
{
    public class WaiterAccountService : IWaiterAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly AuthOptions _authOptions;
        public WaiterAccountService(IAsyncRepository repository, IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authOptions = options.Value;
        }

        public async Task<LogInResponse> LogIn(string userName, string identityId, string notificationTag)
        {
           // var tokens = await _repository.GetTokens(identityId);

            //if (tokens.Count() > 0 && tokens.First().Expires > DateTime.Now)
            //{
            //    await _repository.RemoveToken(tokens.First());
            //}
            //var waiter = await _repository.GetWaiter(identityId);
            //if (!waiter.NotificationTag.Equals(notificationTag))
            //{
            //    waiter.NotificationTag = notificationTag;
            //    await _repository.UpdateWaiter(waiter);
            //}
            return await Authentication(userName, identityId, notificationTag);
        }

        public async Task LogOut(string refreshToken)
        {
            AccountToken accountToken = await _repository.GetToken(refreshToken);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }

        public async Task<Waiter> GetWaiter(string identityId)
        {
            return await _repository.GetWaiter(identityId);
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

            if (identityUser is null)
            {
                return null;
            }
            return await Authentication(identityUser.UserName, identityUser.Id, token.NotificationTag);
        }




        private async Task<LogInResponse> Authentication(string userName, string identityId, string notificationTag)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Waiter")
             //   new Claim("id", admin.Id.ToString())
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
                IdentityUserId = identityId,
                RefreshToken = resp.RefreshToken,
                Expires = DateTime.Now.AddMinutes(_authOptions.RefreshLifetime),
                NotificationTag  = notificationTag
            });
            return resp;
        }
    }
}
