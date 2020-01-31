using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace BR.Services
{
    public class AdminAccountService : IAdminAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly AuthOptions _authOptions;
        public AdminAccountService(IAsyncRepository repository, IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authOptions = options.Value;
        }

        public async Task<LogInResponse> LogIn(string userName, string identityId, string notificationTag)
        {
            return await Authentication(userName, identityId, notificationTag);
        }


        public async Task LogOut(string notificationTag)
        {
            AccountToken accountToken = await _repository.GetTokenByTag(notificationTag);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }

        public async Task LogOut(IdentityUser identityUser)
        {
            if (identityUser is null)
            {
                return;
            }
            var tokens = await _repository.GetTokens(identityUser.Id);
            if (tokens.First() != null)
            {
                await _repository.RemoveToken(tokens.First());
            }
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
            //Admin admin = await _repository.GetAdminById(token.AdminId);            

            if (identityUser is null)
            {
                return null;
            }
            return await Authentication(identityUser.UserName, identityUser.Id, token.NotificationTag);
        }

        public async Task<Admin> GetAdmin(string identityId)
        {
            return await _repository.GetAdminByIdentityId(identityId);
        }

        public async Task<Admin> AddNewAdmin(Admin admin)
        {
            return await _repository.AddAdmin(admin);
        }


        // change notificationTag
        private async Task<LogInResponse> Authentication(string userName, string identityId, string notificationTag)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName), //username
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "Admin")
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
                NotificationTag = notificationTag
            });
            return resp;
        }
    }
}
