using BR.DTO.Account;
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
using System.Text;
using System.Threading.Tasks;

namespace BR.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly AuthOptions _authOptions;
        private readonly IAsyncRepository _repository;

        public AuthenticationService(UserManager<IdentityUser> userManager, 
            IAsyncRepository repository,
            IOptions<AuthOptions> options)
        {
            _userManager = userManager;
            _repository = repository;
            _authOptions = options.Value;
        }

        public async Task<ServerResponse<LogInResponse>> Authentication(string userName, string notificationTag)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
            var identityUser = await _userManager.FindByNameAsync(userName);
            
            if(identityUser is null)
            {
                return new ServerResponse<LogInResponse>(StatusCode.UserNotFound, null);
            }
            var userRoles = await _userManager.GetRolesAsync(identityUser);
            foreach (var item in userRoles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, item));
            }
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
                Expires = DateTime.Now.AddMinutes(_authOptions.RefreshLifetime),
                NotificationTag = notificationTag
            });
            return new ServerResponse<LogInResponse>(resp);
        }

        public async Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken)
        {
            AccountToken token = await _repository.GetToken(refreshToken);
            if (token is null)
            {
                return new ServerResponse<LogInResponse>(StatusCode.TokenError, null);
            }
            if (token.Expires <= DateTime.Now)
            {
                return new ServerResponse<LogInResponse>(StatusCode.Expired, null);
            }
            IdentityUser identityUser = await _repository.GetIdentityUser(token.IdentityUserId);

            if (identityUser is null)
            {
                return new ServerResponse<LogInResponse>(StatusCode.UserNotFound, null);
            }
            return await this.Authentication(identityUser.UserName, token.NotificationTag);
        }

        public string GeneratePassword()
        {
            Random random = new Random();
            string letters = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 8; ++i)
            {
                password.Append(letters.ElementAt(random.Next(0, letters.Length)));
            }
            return password.ToString();
        }
    }
}
