using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IReservationService
    {
        Task AddNewReservation(NewReservationRequest newReservationRequest, string identityId);
        Task<Reservation> CancelReservation(int reservationId);
        Task<Reservation> CompleteReservation(int reservationId);
        Task<Reservation> ChangeTable(int reservationId, IEnumerable<int> newTableIds);
    }
}
