using BR.DTO;
using BR.DTO.Reservations;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ICollection<Reservation>> GetReservations(string identityUserId);
        Task<Reservation> GetReservation(int id);
        Task<ServiceResponse<Reservation>> AddNewReservation(NewReservationRequest newReservationRequest, string identityId);
        Task SendReservationOnConfirmation(NewReservationRequest newReservationRequest, int userId, Client client);
        Task<Reservation> CancelReservation(int reservationId);
        Task<Reservation> CompleteReservation(int reservationId);
        Task ChangeTable(ChangeReservationTablesRequest changeRequest);
    }
}
