using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace BR.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly AuthOptions _authOptions;

        public UserAccountService(IAsyncRepository repository, AuthOptions authOptions)
        {
            _repository = repository;
            _authOptions = authOptions;
        }
        public Task<User> GetInfo(string identityId)
        {
            throw new NotImplementedException();
        }

        public async Task<LogInResponse> LogIn(IdentityUser identityUser)
        {
            return await Authentication(identityUser);
        }

        public Task LogOut(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public Task<LogInResponse> UpdateToken(string refreshToken)
        {
            throw new NotImplementedException();
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
