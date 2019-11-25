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

        public async Task<User> GetInfo(string identityId)
        {
            return await _repository.GetUser(identityId);
        }

        public async Task<LogInUserResponse> LogIn(string userName, string identityId)
        {
            LogInResponse resp = await Authentication(userName, identityId);
            User user = await this._repository.GetUser(identityId);
            return new LogInUserResponse()
            {
                AccessToken = resp.AccessToken,
                RefreshToken = resp.RefreshToken,
                User = user
            };
        }

        public async Task LogOut(string refreshToken)
        {
            AccountToken accountToken = await _repository.GetToken(refreshToken);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }

        public async Task<User> Register(User user)
        {
            user.IsBlocked = false;
            return await _repository.AddUser(user);
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
            return await Authentication(identityUser.UserName, identityUser.Id);
        }

        public async Task<bool> UserIsBlocked(string identityId)
        {
            var user = await _repository.GetUser(identityId);
            if (user != null && !user.IsBlocked)
            {
                return true;
            }
            return false;
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
