using BR.DTO;
using BR.DTO.Account;
using BR.DTO.Users;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace BR.Services
{
    public class UserAccountService : IUserAccountService
    {
        private readonly IAsyncRepository _repository;
        private readonly AuthOptions _authOptions;
        private readonly IBlobService _blobService;

        public UserAccountService(IAsyncRepository repository,
            IOptions<AuthOptions> options,
            IBlobService blobService)
        {
            _repository = repository;
            _authOptions = options.Value;
            _blobService = blobService;
        }

        public string GenerateCode()
        {
            Random random = new Random();
            StringBuilder password = new StringBuilder();
            for (int i = 0; i < 6; ++i)
            {
                password.Append(random.Next(0, 10).ToString());
            }
            return password.ToString();
        }

        public async Task<UserInfoResponse> GetInfo(string identityId)
        {
            var user = await _repository.GetUser(identityId);
            if (user != null)
            {
                return this.UserToUserInfoResponse(user);
            }
            return null;
        }

        public async Task<LogInUserResponse> LogIn(string userName, string identityId, string notificationTag)
        {
            LogInResponse resp = await Authentication(userName, identityId, notificationTag);
            User user = await _repository.GetUser(identityId);
            var res = new LogInUserResponse()
            {
                AccessToken = resp.AccessToken,
                RefreshToken = resp.RefreshToken,
                User = this.UserToUserInfoResponse(user)
            };
            return res;
        }

        public async Task LogOut(string notificationTag)
        {
            AccountToken accountToken = await _repository.GetTokenByTag(notificationTag);
            if (!(accountToken is null))
            {
                await _repository.RemoveToken(accountToken);
            }
        }

        public async Task<UserInfoResponse> Register(User user)
        {
            user.IsBlocked = false;
            user.RegistrationDate = DateTime.Now;
            if(user.ImagePath is null)
            {
                user.ImagePath = "https://rb2020storage.blob.core.windows.net/photos/default-profile.png";
            }
            var userAdded = await _repository.AddUser(user);
            if (userAdded != null)
            {
                return this.UserToUserInfoResponse(userAdded);
            }
            return null;
        }

        public async Task<LogInResponse> UpdateToken(string refreshToken)
        {
            AccountToken token = await _repository.GetToken(refreshToken);
            if (token is null)
            {
                return null;
            }
            if (token.Expires <= DateTime.Now)
            {
                return null;
            }
            IdentityUser identityUser = await _repository.GetIdentityUser(token.IdentityUserId);

            if (identityUser is null)
            {
                return null;
            }
            return await Authentication(identityUser.UserName, identityUser.Id, token.NotificationTag);
        }


        public async Task<bool> UserIsBlocked(string identityId)
        {
            var user = await _repository.GetUser(identityId);
            if (user != null && user.IsBlocked)
            {
                return true;
            }
            return false;
        }


        public async Task<string> UploadImage(string identityId, string imageString)
        {
            var user = await _repository.GetUser(identityId);
            var path = await _blobService.UploadImage(imageString);
            user.ImagePath = path;
            await _repository.UpdateUser(user);
            return path;
        }

        private async Task<LogInResponse> Authentication(string userName, string identityId, string notificationTag)
        {
            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName),
                new Claim(ClaimsIdentity.DefaultRoleClaimType, "User")
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
                AccessToken = tokenStr,
                RefreshToken = Guid.NewGuid().ToString()
            };

            await _repository.AddToken(new AccountToken()
            {
                IdentityUserId = identityId,
                RefreshToken = resp.RefreshToken,
                Expires = DateTime.Now.AddMinutes(_authOptions.RefreshLifetime),
                NotificationTag = notificationTag
            });
            return resp;
        }

        public async Task<UserInfoResponse> UpdateProfile(UpdateUserRequest updateUserRequest, string identityId)
        {
            var userToUpdate = await _repository.GetUser(identityId);
            userToUpdate.FirstName = updateUserRequest.FirstName;
            userToUpdate.LastName = updateUserRequest.LastName;
            userToUpdate.Gender = updateUserRequest.Gender;
            if (updateUserRequest.BirthDate != null)
            {
                userToUpdate.BirthDate = DateTime.ParseExact(updateUserRequest.BirthDate, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            }
            else
            {
                userToUpdate.BirthDate = null;
            }
            try
            {
                var user = await _repository.UpdateUser(userToUpdate);
                if (user is null)
                {
                    return null;
                }
                return this.UserToUserInfoResponse(user);

            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> DeleteUser(string identityId)
        {
            var user = await _repository.GetUser(identityId);
            if (user != null)
            {
                return await _repository.DeleteUser(user);

            }
            return false;
        }

        private UserInfoResponse UserToUserInfoResponse(User user)
        {
            if (user is null)
            {
                return null;
            }
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

                    var title = res.TableReservations.First().Table.Hall.Floor.Client.Name;
                    var floor = res.TableReservations.First().Table.Hall.Floor.Number;
                    var hallTitle = res.TableReservations.First().Table.Hall.Title;

                    var resInfo = new ReservationInfo()
                    {
                        Id = res.Id,
                        Date = res.ReservationDate,
                        ReservationState = res.ReservationState is null ? "idle" : res.ReservationState.Title,
                        ChildFree = res.ChildFree,
                        ClientTitle = res.TableReservations.First().Table.Hall.Floor.Client.Name,
                        Floor = res.TableReservations.First().Table.Hall.Floor.Number,
                        HallTitle = res.TableReservations.First().Table.Hall.Title,
                        GuestCount = res.GuestCount,
                        TableNumbers = tableNums,
                        Comments = res.Comments
                    };
                    reservations.Add(resInfo);
                }
            }
            return new UserInfoForUsersResponse()
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
