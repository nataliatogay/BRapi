using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BR.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly AuthOptions _authOptions;

        public UserAccountService(IAsyncRepository repository, IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authOptions = options.Value;
        }

        public string GenerateCode()
        {
            Random random = new Random();
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 6; ++i)
            {
                password.Append(random.Next(0, 10).ToString());
            }
            return password.ToString();
        }

        public Task<User> GetInfo(string identityId)
        {
            throw new NotImplementedException();
        }

        public async Task<LogInResponse> LogIn(string userName, string identityId)
        {
            return await Authentication(userName, identityId);
        }

        public Task LogOut(string refreshToken)
        {
            throw new NotImplementedException();
        }

        public async Task<User> Register(User user)
        {
            return await _repository.AddUser(user);
        }

        public Task<LogInResponse> UpdateToken(string refreshToken)
        {
            throw new NotImplementedException();
        }

        private async Task<LogInResponse> Authentication(string userName, string identityId)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
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
                Expires = DateTime.Now.AddMinutes(_authOptions.RefreshLifetime)
            });
            return resp;
        }
    }
}
