using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Clients;
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
        private readonly IClientService _clientService;

        public ClientAccountService(IAsyncRepository repository,
            IBlobService blobService,
            IAuthenticationService authenticationService,
            IClientService clientService)
        {
            _repository = repository;
            _blobService = blobService;
            _authenticationService = authenticationService;
            _clientService = clientService;
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

        public async Task<ServerResponse<bool>> ClientIsBlocked(string identityId)
        {
            var client = await _repository.GetClient(identityId);
            if (client is null)
            {
                return new ServerResponse<bool>(StatusCode.UserNotFound, false);
            }
            if (client.Blocked is null)
            {
                return new ServerResponse<bool>(StatusCode.Ok, false);
            }
            else
            {
                return new ServerResponse<bool>(StatusCode.Ok, true);
            }
        }

        public async Task<ServerResponse<bool>> ClientIsDeleted(string identityId)
        {
            var client = await _repository.GetClient(identityId);
            if (client is null)
            {
                return new ServerResponse<bool>(StatusCode.UserNotFound, false);
            }
            if (client.Deleted is null)
            {
                return new ServerResponse<bool>(StatusCode.Ok, false);
            }
            else
            {
                return new ServerResponse<bool>(StatusCode.Ok, true);
            }
        }

        public async Task<ServerResponse<string>> UploadMainImage(string identityId, string imageString)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse<string>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            return await _clientService.UploadMainImage(new UploadMainImageRequest()
            {
                ClientId = client.Id,
                ImageString = imageString
            });
        }

        public async Task<ServerResponse> UploadImages(string identityId, ICollection<string> imagesString)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityId);
                if (client is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }

            return await _clientService.UploadImages(new UploadImagesRequest()
            {
                ClientId = client.Id,
                ImageStrings = imagesString
            });
        }

        public async Task<ServerResponse> SetAsMainImage(int imageId)
        {
            return await _clientService.SetAsMainImage(imageId);
        }

        public async Task<ServerResponse> DeleteImage(int imageId)
        {
            return await _clientService.DeleteImage(imageId);
        }

        public async Task<ServerResponse> UpdateClient(UpdateClientRequest updateRequest, string identityIdClient)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(identityIdClient);
                if (client is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            updateRequest.ClientId = client.Id;

            return await _clientService.UpdateClient(updateRequest);

        }

    }
}
