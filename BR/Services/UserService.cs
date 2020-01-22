using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;

namespace BR.Services
{
    public class UserService : IUserService
    {
        private readonly IAsyncRepository _repository;

        public UserService(IAsyncRepository repository)
        {
            _repository = repository;

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

        public async Task<UserInfoResponse> GetUser(int id)
        {
            var user = await _repository.GetUser(id);
            if (user is null)
            {
                return null;
            }
            return this.UserToUserInfoResponse(user);
        }

        public async Task<User> BlockUser(int id)
        {
            var user = await _repository.GetUser(id);
            if (user is null)
            {
                return null;
            }
            user.IsBlocked = true;
            return await _repository.UpdateUser(user);
        }

        private UserInfoResponse UserToUserInfoResponse(User user)
        {
            var reservations = new List<ReservationInfo>();
            if (user.Reservations != null)
            {
                foreach (var res in user.Reservations)
                {
                    var tableNums = new List<int>();
                    if (res.TableReservations != null)
                    {
                        foreach (var t in res.TableReservations)
                        {
                            tableNums.Add(t.Table.Number);
                        }
                    }

                    var resInfo = new ReservationInfo()
                    {
                        Date = res.ReservationDate,
                        ReservationState = res.ReservationState.Title,
                        ChildFree = res.ChildFree,
                        ClientTitle = res.TableReservations.First().Table.Hall.Floor.Client.Name,
                        Floor = res.TableReservations.First().Table.Hall.Floor.Number,
                        HallTitle = res.TableReservations.First().Table.Hall.Title,
                        GuestCount = res.GuestCount,
                        TableNumbers = tableNums
                    };
                    reservations.Add(resInfo);
                }
            }
            return new UserInfoResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImagePath = user.ImagePath,
                Email = user.Identity.Email,
                Reservations = reservations
            };
        }
    }
}
