using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Admin;
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
        private readonly IAuthenticationService _authenticationService;
        public AdminAccountService(IAsyncRepository repository,
             IAuthenticationService authenticationService,
             IOptions<AuthOptions> options)
        {
            _repository = repository;
            _authenticationService = authenticationService;
            _authOptions = options.Value;
        }

        public async Task<ServerResponse<LogInResponse>> LogIn(string userName, string notificationTag)
        {
            return await _authenticationService.Authentication(userName, notificationTag);
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

        public async Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken)
        {
            return await _authenticationService.UpdateToken(refreshToken);
        }

        public async Task<ServerResponse<AdminInfoResponse>> GetAdminInfo(string identityId)
        {
            Admin admin;
            try
            {
                admin = await _repository.GetAdmin(identityId);
                if (admin is null)
                {
                    return new ServerResponse<AdminInfoResponse>(StatusCode.UserNotFound, null);
                }
                return new ServerResponse<AdminInfoResponse>(
                    StatusCode.Ok,
                    new AdminInfoResponse()
                    {
                        Email = admin.Identity.Email
                    });
            }
            catch
            {
                return new ServerResponse<AdminInfoResponse>(StatusCode.DbConnectionError, null);
            }
        }

        public async Task<Admin> AddNewAdmin(Admin admin)
        {
            return await _repository.AddAdmin(admin);
        }

    }
}
