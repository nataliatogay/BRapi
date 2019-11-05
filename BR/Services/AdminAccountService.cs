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
    public class AdminAccountService : IAdminAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly AuthOptions _authOptions;
        public AdminAccountService(IAsyncRepository repository, IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authOptions = options.Value;
        }            

        public async Task<LogInResponse> LogIn(string email, string password)
        {
            Admin admin = await _repository.GetAdminByEmail(email);
            
            if (admin is null)
            {
                return null;
            }
            return await Authentication(admin);
        }


        public async Task LogOut(string refreshToken)
        {            
            AdminAccountToken accountToken = await _repository.GetAdminToken(refreshToken);
            if (!(accountToken is null))
            {
                await _repository.RemoveAdminToken(accountToken);
            }
        }

        public async Task<LogInResponse> UpdateToken(string refreshToken)
        {
            AdminAccountToken token = await _repository.GetAdminToken(refreshToken);
            if (token is null)
            {
                return null;
            }
            if (token.Expires <= DateTime.Now)
            {
                return null;
            }
            Admin admin = await _repository.GetAdminById(token.AdminId);            

            if (admin is null)
            {
                return null;
            }
            return await Authentication(admin);
        }

        public async Task<Admin> GetInfo(int id)
        {
            return await _repository.GetAdminById(id);
        }

        public async Task<Admin> AddNewAdmin(IdentityUser user)
        {
            Admin admin = new Admin()
            {
                Email = user.Email,
                Password = user.PasswordHash,
                IdentityId = user.Id
            };
            return await _repository.AddAdmin(admin);
        }


        private async Task<LogInResponse> Authentication(Admin admin)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, admin.Email), //username
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
                Email = admin.Email,
                AccessToken = tokenStr,
                RefreshToken = Guid.NewGuid().ToString()
            };

            await _repository.AddAdminToken(new AdminAccountToken()
            {
                AdminId = admin.Id,
                RefreshToken = resp.RefreshToken,
                Expires = DateTime.Now.AddMinutes(_authOptions.RefreshLifetime)
            });            
            return resp;
        }
    }
}
