using System;
using System.Collections.Generic;

namespace BR.DTO.Users
{
    public class UserInfoResponse
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool? Gender { get; set; }
        public string ImagePath { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Email { get; set; }
    }

    public class UserInfoForUsersResponse : UserInfoResponse
    {
        public ICollection<ReservationInfo> Reservations { get; set; }
    }

    public class UserInfoForAdminResponse : UserInfoResponse
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime RegistrationDate { get; set; }
    }

    public class ReservationInfo
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public int GuestCount { get; set; }
        public bool ChildFree { get; set; }
        public string ReservationState { get; set; }
        public string ClientTitle { get; set; }
        public string HallTitle { get; set; }
        public int Floor { get; set; }
        public ICollection<int> TableNumbers { get; set; }
        public string Comments { get; set; }
    }
}
