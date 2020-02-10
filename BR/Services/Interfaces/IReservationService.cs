using BR.DTO;
using BR.DTO.Reservations;
using BR.Models;
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
        Task<Reservation> AddNewReservation(NewReservationRequest newReservationRequest, string identityId);
        Task<Reservation> CancelReservation(int reservationId);
        Task<Reservation> CompleteReservation(int reservationId);
        Task ChangeTable(ChangeReservationTablesRequest changeRequest);
    }
}
