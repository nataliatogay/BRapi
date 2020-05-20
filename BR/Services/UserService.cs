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

        public UserService(IAsyncRepository repository)
        {
            _repository = repository;

        }


        public async Task<ServerResponse<ICollection<UserShortInfoForAdmin>>> GetUserShortInfoForAdmin()
        {
            ICollection<User> users;
            try
            {
                users = await _repository.GetUsers();
            }
            catch
            {
                return new ServerResponse<ICollection<UserShortInfoForAdmin>>(StatusCode.Error, null);
            }
            if (users is null)
                return new ServerResponse<ICollection<UserShortInfoForAdmin>>(StatusCode.Ok, null);

            var res = new List<UserShortInfoForAdmin>();
            foreach (var user in users)
            {
                res.Add(new UserShortInfoForAdmin()
                {
                    Email = user.Identity.Email,
                    FirstName = user.FirstName,
                    Id = user.Id,
                    ImagePath = user.ImagePath,
                    Blocked = user.Blocked,
                    Deleted = user.Deleted,
                    LastName = user.LastName,
                    PhoneNumber = user.Identity.PhoneNumber,
                    RegistrationDate = user.RegistrationDate
                });
            }
            return new ServerResponse<ICollection<UserShortInfoForAdmin>>(StatusCode.Ok, res);
        }


        public async Task<ServerResponse<UserInfoForAdmin>> GetUserInfoForAdmin(int id)
        {
            try
            {
                var user = await _repository.GetUser(id);
                if (user is null)
                {
                    return new ServerResponse<UserInfoForAdmin>(StatusCode.UserNotFound, null);
                }
                var res = new UserInfoForAdmin()
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
                return new ServerResponse<UserInfoForAdmin>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<UserInfoForAdmin>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<ICollection<UserShortInfoForClient>>> GetAllVisitorsByClient(string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.DbConnectionError, null);
            }

            var userIdentities = new List<string>();
            foreach (var item in client.Reservations)
            {
                if (!userIdentities.Contains(item.IdentityUserId))
                {
                    userIdentities.Add(item.IdentityUserId);
                }

            }

            var res = new List<UserShortInfoForClient>();
            try
            {

                foreach (var item in userIdentities)
                {
                    var user = await _repository.GetUser(item);
                    if (user != null)
                    {
                        res.Add(new UserShortInfoForClient()
                        {
                            Id = user.Id,
                            BirthDate = user.BirthDate,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneNumber = user.Identity.PhoneNumber,
                            RegistrationDate = user.RegistrationDate,
                            ImagePath = user.ImagePath is null ? "https://rb2020storage.blob.core.windows.net/photos/default-profile.png" : user.ImagePath
                        });
                    }
                }
            }
            catch
            {
                return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.DbConnectionError, null);
            }
            return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ICollection<UserShortInfoForClient>>> GetAllVisitorsByOwner(string ownerIdentityId, int clientId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.DbConnectionError, null);
            }


            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.NotFound, null);
            }

            var userIdentities = new List<string>();
            foreach (var item in client.Reservations)
            {
                if (!userIdentities.Contains(item.IdentityUserId))
                {
                    userIdentities.Add(item.IdentityUserId);
                }

            }

            var res = new List<UserShortInfoForClient>();
            try
            {
                foreach (var item in userIdentities)
                {
                    var user = await _repository.GetUser(item);
                    if (user != null)
                    {
                        res.Add(new UserShortInfoForClient()
                        {
                            Id = user.Id,
                            BirthDate = user.BirthDate,
                            FirstName = user.FirstName,
                            LastName = user.LastName,
                            PhoneNumber = user.Identity.PhoneNumber,
                            RegistrationDate = user.RegistrationDate,
                            ImagePath = user.ImagePath is null ? "https://rb2020storage.blob.core.windows.net/photos/default-profile.png" : user.ImagePath
                        });
                    }
                }
            }
            catch
            {
                return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.DbConnectionError, null);
            }
            return new ServerResponse<ICollection<UserShortInfoForClient>>(StatusCode.Ok, res);

        }

        public async Task<ServerResponse<UserFullInfoForClient>> GetUserFullInfoByClient(string clientIdentityId, int userId)
        {
            Client client;
            User user;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                user = await _repository.GetUser(userId);
                if (client is null)
                {
                    return new ServerResponse<UserFullInfoForClient>(StatusCode.UserNotFound, null);
                }
                if (user is null || client.Reservations.FirstOrDefault(item => item.IdentityUserId.Equals(user.IdentityId)) is null)
                {
                    return new ServerResponse<UserFullInfoForClient>(StatusCode.NotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<UserFullInfoForClient>(StatusCode.DbConnectionError, null);
            }

            UserFullInfoForClient result;

            try
            {
                result = await this.UserToUserFullInfoForClient(user, client.Id);
                return new ServerResponse<UserFullInfoForClient>(StatusCode.Ok, result);
            }
            catch
            {
                return new ServerResponse<UserFullInfoForClient>(StatusCode.DbConnectionError, null);
            }
        }


        public async Task<ServerResponse<UserFullInfoForClient>> GetUserFullInfoByOwner(string ownerIdentityId, int clientId, int userId)
        {
            Owner owner;
            User user;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                user = await _repository.GetUser(userId);
                Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);
                if (owner is null)
                {
                    return new ServerResponse<UserFullInfoForClient>(StatusCode.UserNotFound, null);
                }

                if ( user is null || client is null || client.Reservations.FirstOrDefault(item => item.IdentityUserId.Equals(user.IdentityId)) is null)
                {
                    return new ServerResponse<UserFullInfoForClient>(StatusCode.NotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<UserFullInfoForClient>(StatusCode.DbConnectionError, null);
            }

            UserFullInfoForClient result;

            try
            {
                result = await this.UserToUserFullInfoForClient(user, clientId);
                return new ServerResponse<UserFullInfoForClient>(StatusCode.Ok, result);
            }
            catch
            {
                return new ServerResponse<UserFullInfoForClient>(StatusCode.DbConnectionError, null);
            }

        }



        private async Task<UserFullInfoForClient> UserToUserFullInfoForClient(User user, int clientId)
        {
            if (user is null)
            {
                return null;
            }
            var reservations = await _repository.GetAllUserReservations(user.IdentityId);
            var clientReservations = reservations.Where(item => item.ClientId == clientId);
            var resStateCompleted = await _repository.GetReservationState("Completed");
            var lastVisit = clientReservations.Where(item => item.ReservationDate < DateTime.Now && item.ReservationStateId != null && item.ReservationStateId == resStateCompleted.Id).OrderByDescending(item => item.ReservationDate).FirstOrDefault();
            var result = new UserFullInfoForClient()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImagePath = user.ImagePath is null ? "https://rb2020storage.blob.core.windows.net/photos/default-profile.png" : user.ImagePath,
                PhoneNumber = user.Identity.PhoneNumber,
                Email = user.Identity.Email,
                RegistrationDate = user.RegistrationDate,
                ReservationCount = clientReservations.Count()
            };

            //result.LastVisitDate = (lastVisit == null ? null : lastVisit.ReservationDate);

            if (lastVisit is null)
            {
                result.LastVisitDate = null;
            }
            else
            {
                result.LastVisitDate = lastVisit.ReservationDate;
            }

            return result;
        }




        // ==============================================================================





        public async Task<ServerResponse<UserInfoForUsers>> GetUserInfoForUsers(int id)
        {
            try
            {
                var user = await _repository.GetUser(id);
                if (user is null)
                {
                    return new ServerResponse<UserInfoForUsers>(StatusCode.UserNotFound, null);
                }
                var res = new UserInfoForUsers()
                {
                    BirthDate = user.BirthDate,
                    Email = user.Identity.Email,
                    FirstName = user.FirstName,
                    Gender = user.Gender,
                    ImagePath = user.ImagePath,
                    LastName = user.LastName
                };
                return new ServerResponse<UserInfoForUsers>(StatusCode.Ok, res);
            }
            catch
            {
                return new ServerResponse<UserInfoForUsers>(StatusCode.Error, null);
            }
        }

        // CHANGE
        public async Task<ServerResponse> BlockUser(int userId)
        {
            //User user;
            //try
            //{
            //    user = await _repository.GetUser(userId);
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //if (user is null)
            //{
            //    return new ServerResponse(StatusCode.UserNotFound);
            //}
            //if (user.Deleted != null)
            //{
            //    return new ServerResponse(StatusCode.UserDeleted);
            //}
            //if (user.Blocked is null)
            //{
            //    user.Blocked = DateTime.Now;
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
            //        var cancelReason = await _repository.GetCancelReason("UserBlocked");

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
            //else
            //{
            //    return new ServerResponse(StatusCode.UserBlocked);
            //}

            return new ServerResponse(StatusCode.Ok); // <- delete this
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
            if (user.Deleted != null)
            {
                return new ServerResponse(StatusCode.UserDeleted);
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
