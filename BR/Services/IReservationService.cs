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
        Task CancelReservation(int reservationId);
        Task CompleteReservation(int reservationId);
        Task ChangeTable(int reservationId, IEnumerable<int> tableIds);
    }
}
