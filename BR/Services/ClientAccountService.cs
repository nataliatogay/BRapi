﻿using System;
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
        private readonly IAuthenticationService _authenticationService;

        public ClientAccountService(IAsyncRepository repository,
            IBlobService blobService,
            IAuthenticationService authenticationService)
        {
            _repository = repository;
            _blobService = blobService;
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

        public async Task<Client> GetInfo(string identityId)
        {
            return await _repository.GetClient(identityId);
        }

        public async Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken)
        {
            return await _authenticationService.UpdateToken(refreshToken);
        }


        public async Task<bool> ClientIsBlocked(string identityId)
        {
            // TODO

            //var client = await _repository.GetClient(identityId);
            //if (client != null && client.IsBlocked)
            //{
            //    return true;
            //}
            return false;
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
            if (img is null)
            {
                return false;
            }
            return await _blobService.DeleteImage(img.ImagePath);
        }
    }
}
