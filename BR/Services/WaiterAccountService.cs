using BR.DTO;
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
using System.Threading.Tasks;

namespace BR.Services
{
    public class WaiterAccountService : IWaiterAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly IAuthenticationService _authenticationService;
        public WaiterAccountService(IAsyncRepository repository,
            IAuthenticationService authenticationService)
        {
            _repository = repository;
            _authenticationService = authenticationService;
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

        public async Task<Waiter> GetWaiter(string identityId)
        {
            return await _repository.GetWaiter(identityId);
        }


        public async Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken)
        {
            return await _authenticationService.UpdateToken(refreshToken);
        }

    }
}
