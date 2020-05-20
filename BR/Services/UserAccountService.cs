using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Reservations;
using BR.DTO.Users;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using BR.Utils.Notification;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly IBlobService _blobService;
        private readonly IAuthenticationService _authenticationService;
        private readonly IReservationService _reservationService;
        private readonly IPushNotificationService _notificationService;


        public UserAccountService(IAsyncRepository repository,
            IAuthenticationService authenticationService,
            IReservationService reservationService,
            IPushNotificationService notificationService,
            IBlobService blobService)
        {
            _repository = repository;
            _authenticationService = authenticationService;
            _reservationService = reservationService;
            _blobService = blobService;
            _notificationService = notificationService;

        }

        public async Task<ServerResponse<LogInUserResponse>> LogIn(string userName, string identityId, string notificationTag)
        {
            var resp = await _authenticationService.Authentication(userName, notificationTag);
            if (resp.StatusCode == StatusCode.Ok)
            {
                User user;
                try
                {
                    user = await _repository.GetUser(identityId);
                }
                catch
                {
                    return new ServerResponse<LogInUserResponse>(StatusCode.DbConnectionError, null);
                }

                var res = new LogInUserResponse()
                {
                    AccessToken = resp.Data.AccessToken,
                    RefreshToken = resp.Data.RefreshToken,
                    User = this.UserToUserInfoResponse(user)
                };
                return new ServerResponse<LogInUserResponse>(res);
            }

            return new ServerResponse<LogInUserResponse>(resp.StatusCode, null);
        }

        public async Task LogOut(string notificationTag)
        {
            AccountToken accountToken = await _repository.GetTokenByTag(notificationTag);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }

        public async Task<ServerResponse<UserInfoForUsers>> Register(NewUserRequest newUserRequest, string identityUserId)
        {
            User user = new User()
            {
                IdentityId = identityUserId,
                FirstName = newUserRequest.FirstName,
                LastName = newUserRequest.LastName,
                Gender = newUserRequest.Gender,
                BirthDate = DateTime.ParseExact(newUserRequest.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                RegistrationDate = DateTime.Now,
                ImagePath = null
            };

            try
            {

                var userAdded = await _repository.AddUser(user);
                return new ServerResponse<UserInfoForUsers>(StatusCode.Ok, this.UserToUserInfoResponse(userAdded));
            }
            catch
            {
                return new ServerResponse<UserInfoForUsers>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<LogInResponse>> UpdateToken(string refreshToken)
        {
            return await _authenticationService.UpdateToken(refreshToken);
        }


        public async Task<ServerResponse<bool>> UserIsBlocked(string identityId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(identityId);
                if (user is null)
                {
                    return new ServerResponse<bool>(StatusCode.UserNotFound, false);
                }
            }
            catch
            {
                return new ServerResponse<bool>(StatusCode.DbConnectionError, false);
            }
            if (user.Blocked is null)
            {
                return new ServerResponse<bool>(StatusCode.Ok, false);
            }
            else
            {
                return new ServerResponse<bool>(StatusCode.Ok, true);
            }
        }

        public async Task<ServerResponse<bool>> UserIsDeleted(string identityId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(identityId);
                if (user is null)
                {
                    return new ServerResponse<bool>(StatusCode.UserNotFound, false);
                }
            }
            catch
            {
                return new ServerResponse<bool>(StatusCode.DbConnectionError, false);
            }
            if (user.Deleted is null)
            {
                return new ServerResponse<bool>(StatusCode.Ok, false);
            }
            else
            {
                return new ServerResponse<bool>(StatusCode.Ok, true);
            }
        }

        public async Task<ServerResponse<bool>> UserIsRegistered(string identityId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(identityId);
                if (user is null)
                {
                    return new ServerResponse<bool>(StatusCode.Ok, false);
                }
                else
                {
                    return new ServerResponse<bool>(StatusCode.Ok, true);
                }
            }
            catch
            {
                return new ServerResponse<bool>(StatusCode.DbConnectionError, false);
            }
        }


        public async Task<ServerResponse<string>> UploadImage(string identityId, string imageString)
        {
            User user;
            try
            {
                user = await _repository.GetUser(identityId);

                if (user is null)
                {
                    return new ServerResponse<string>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.DbConnectionError, null);
            }
            try
            {
                var path = await _blobService.UploadImage(imageString);
                user.ImagePath = path;
                await _repository.UpdateUser(user);
                return new ServerResponse<string>(StatusCode.Ok, path);
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse> DeleteImage(string identityId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(identityId);
                if (user is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

            try
            {
                await _blobService.DeleteImage(user.ImagePath);
            }
            catch { }
            try
            {
                user.ImagePath = null;
                await _repository.UpdateUser(user);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }

        }

        public async Task<ServerResponse<UserInfoForUsers>> UpdateProfile(UpdateUserRequest updateUserRequest, string identityId)
        {
            User userToUpdate;
            try
            {
                userToUpdate = await _repository.GetUser(identityId);
                if (userToUpdate is null)
                {
                    return new ServerResponse<UserInfoForUsers>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<UserInfoForUsers>(StatusCode.DbConnectionError, null);
            }

            userToUpdate.FirstName = updateUserRequest.FirstName;
            userToUpdate.LastName = updateUserRequest.LastName;
            userToUpdate.Gender = updateUserRequest.Gender;
            userToUpdate.BirthDate = DateTime.ParseExact(updateUserRequest.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            try
            {
                var user = await _repository.UpdateUser(userToUpdate);
                return new ServerResponse<UserInfoForUsers>(StatusCode.Ok, this.UserToUserInfoResponse(user));

            }
            catch
            {
                return new ServerResponse<UserInfoForUsers>(StatusCode.DbConnectionError, null);
            }
        }


        private UserInfoForUsers UserToUserInfoResponse(User user)
        {
            if (user is null)
            {
                return null;
            }
            return new UserInfoForUsers()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImagePath = user.ImagePath,
                Email = user.Identity.Email,

            };

        }


        //=================================================================================================



        // CHANGE
        // send notification to clients
        public async Task<ServerResponse> DeleteUser(string identityId)
        {
            //var user = await _repository.GetUser(identityId);
            //if (user != null)
            //{
            //    user.Deleted = DateTime.Now;
            //    try
            //    {
            //        await _repository.UpdateUser(user);

            //        // delete tokens
            //        var tokens = await _repository.GetTokens(user.IdentityId);
            //        if (tokens != null)
            //        {
            //            foreach (var item in tokens)
            //            {
            //                await _repository.RemoveToken(item);
            //            }
            //        }

            //        // cancel upcoming reservations
            //        var reservations = user.Reservations;
            //        var cancelReason = await _repository.GetCancelReason("UserDeleted");

            //        // if closes admin
            //        //var admin = (await _repository.GetAdmins()).FirstOrDefault();
            //        if (cancelReason != null && reservations != null)
            //        {
            //            foreach (var item in reservations)
            //            {
            //                if (item.ReservationDate > DateTime.Now && item.ReservationStateId is null)
            //                {
            //                    await _reservationService.CancelReservation(item.Id, cancelReason.Id, user.IdentityId);
            //                }
            //            }
            //        }

            //        return new ServerResponse(StatusCode.Ok);
            //    }
            //    catch
            //    {
            //        return new ServerResponse(StatusCode.Error);
            //    }
            //}
            //return new ServerResponse(StatusCode.UserNotFound);

            return new ServerResponse(StatusCode.Ok); // <- delete this
        }

        public async Task<ServerResponse> RestoreUser(string identityId)
        {
            var user = await _repository.GetUser(identityId);
            if (user is null)
            {
                return new ServerResponse(StatusCode.UserNotFound);
            }
            user.Deleted = null;
            try
            {
                await _repository.UpdateUser(user);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
        }

        public async Task<ServerResponse> FinallyDelete(string notificationTag)
        {
            AccountToken token;
            try
            {
                token = await _repository.GetTokenByTag(notificationTag);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (token != null)
            {
                try
                {
                    await _repository.RemoveToken(token);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }
            }
            return new ServerResponse(StatusCode.Error);
        }




    }
}
