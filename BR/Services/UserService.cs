using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.DTO.Reservations;
using BR.DTO.Users;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;

namespace BR.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository _repository;
        private readonly IReservationService _reservationService;

        public UserService(IAsyncRepository repository,
            IReservationService reservationService)
        {
            _repository = repository;
            _reservationService = reservationService;

        }
        public async Task<ServerResponse<ICollection<UserInfoForAdminResponse>>> GetUsers()
        {
            ICollection<User> users;
            try
            {
                users = await _repository.GetUsers();
            }
            catch
            {
                return new ServerResponse<ICollection<UserInfoForAdminResponse>>(StatusCode.Error, null);
            }
            if (users is null)
                return new ServerResponse<ICollection<UserInfoForAdminResponse>>(StatusCode.Ok, null);

            var res = new List<UserInfoForAdminResponse>();
            foreach (var user in users)
            {
                res.Add(new UserInfoForAdminResponse()
                {
                    BirthDate = user.BirthDate,
                    Email = user.Identity.Email,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    Id = user.Id,
                    ImagePath = user.ImagePath,
                    Blocked = user.Blocked,
                    Deleted = user.Deleted,
                    LastName = user.LastName,
                    PhoneNumber = user.Identity.PhoneNumber,
                    RegistrationDate = user.RegistrationDate
                });
            }
            return new ServerResponse<ICollection<UserInfoForAdminResponse>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<UserInfoForAdminResponse>> GetUserInfoForAdmin(int id)
        {
            try
            {
                var user = await _repository.GetUser(id);
                if (user is null)
                {
                    return new ServerResponse<UserInfoForAdminResponse>(StatusCode.UserNotFound, null);
                }
                var res = new UserInfoForAdminResponse()
                {
                    BirthDate = user.BirthDate,
                    Blocked = user.Blocked,
                    Deleted = user.Deleted,
                    Email = user.Identity.Email,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    Id = user.Id,
                    ImagePath = user.ImagePath,
                    LastName = user.LastName,
                    PhoneNumber = user.Identity.PhoneNumber,
                    RegistrationDate = user.RegistrationDate
                };
                return new ServerResponse<UserInfoForAdminResponse>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<UserInfoForAdminResponse>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<UserInfoForUsersResponse>> GetUserInfoForUsers(int id)
        {
            try
            {
                var user = await _repository.GetUser(id);
                if (user is null)
                {
                    return new ServerResponse<UserInfoForUsersResponse>(StatusCode.UserNotFound, null);
                }
                var res = new UserInfoForUsersResponse()
                {
                    BirthDate = user.BirthDate,
                    Email = user.Identity.Email,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    ImagePath = user.ImagePath,
                    LastName = user.LastName
                };
                return new ServerResponse<UserInfoForUsersResponse>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<UserInfoForUsersResponse>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse> BlockUser(int userId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(userId);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
            if (user is null)
            {
                return new ServerResponse(StatusCode.UserNotFound);
            }
            if (user.Blocked is null)
            {
                user.Blocked = DateTime.Now;
                try
                {
                    await _repository.UpdateUser(user);

                    // delete tokens
                    var tokens = await _repository.GetTokens(user.IdentityId);
                    if (tokens != null)
                    {
                        foreach (var item in tokens)
                        {
                            await _repository.RemoveToken(item);
                        }
                    }

                    // cancel upcoming reservations
                    var reservations = user.Reservations;
                    var cancelReason = await _repository.GetCancelReason("UserDeleted");

                    // if closes admin
                    //var admin = (await _repository.GetAdmins()).FirstOrDefault();
                    if (cancelReason != null && reservations != null)
                    {
                        foreach (var item in reservations)
                        {
                            if (item.ReservationDate > DateTime.Now && item.ReservationStateId is null)
                            {
                                await _reservationService.CancelReservation(item.Id, cancelReason.Id, user.IdentityId);
                            }
                        }
                    }
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.UserBlocked);
            }
        }

        public async Task<ServerResponse> UnblockUser(int userId)
        {
            User user;
            try
            {
                user = await _repository.GetUser(userId);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
            if (user is null)
            {
                return new ServerResponse(StatusCode.UserNotFound);
            }
            if (user.Blocked != null)
            {
                user.Blocked = null;
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
            else
            {
                return new ServerResponse(StatusCode.UserUnblocked);
            }
        }

    }
}
