using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;

namespace BR.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository _repository;
        private readonly IBlobService _blobService;

        public UserService(IAsyncRepository repository,
            IBlobService blobService)
        {
            _repository = repository;
            _blobService = blobService;
        }
        public async Task<IEnumerable<UserInfoResponse>> GetUsers()
        {
            var users = await _repository.GetUsers();
            if (users is null)
                return null;

            var res = new List<UserInfoResponse>();            
            foreach (var user in users)
            {
                res.Add(this.UserToUserInfoResponse(user));
            }
            return res;
        }

        public async Task<string> UploadImage(string identityId, string imageString)
        {
            var user = await _repository.GetUser(identityId);
            var path = await _blobService.UploadImage(imageString);
            user.ImagePath = path;
            await _repository.UpdateUser(user);
            return path;
        }

        public async Task<UserInfoResponse> UpdateUser(UpdateUserRequest updateUserRequest, string identityId)
        {
            var userToUpdate = await _repository.GetUser(identityId);
            userToUpdate.FirstName = updateUserRequest.FirstName;
            userToUpdate.LastName = updateUserRequest.LastName;
            userToUpdate.Gender = updateUserRequest.Gender;
            if(updateUserRequest.BirthDate != null)
            {
                userToUpdate.BirthDate = DateTime.ParseExact(updateUserRequest.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            var user = await _repository.UpdateUser(userToUpdate);
            return this.UserToUserInfoResponse(user);
        }

        private UserInfoResponse UserToUserInfoResponse(User user)
        {
            return new UserInfoResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImagePath = user.ImagePath,
                Email = user.Identity.Email
                
            };
        }
    }
}
