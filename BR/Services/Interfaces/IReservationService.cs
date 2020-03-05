using BR.DTO;
using BR.DTO.Reservations;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BR.DTO.Redis;

namespace BR.Services.Interfaces
{
    public interface IReservationService
    {
        Task<ICollection<Reservation>> GetReservations(string identityUserId);
        Task<Reservation> GetReservation(int id);
        Task<ServerResponse> SetPendingTableState(TableStatesRequests stateRequest);
        Task<ServerResponse<Reservation>> AddNewReservation(NewReservationRequest newReservationRequest, string identityId);
        Task<ServerResponse> SendReservationOnConfirmation(NewReservationRequest newReservationRequest, int userId, Client client);
        Task<ServerResponse> AddConfirmedReservation(ConfirmReservationRequest confirmRequest);
        Task<ServerResponse> AddReservationByPhone(NewReservationByPhoneRequest reservationRequest, string waiterIdentityId);
        Task<Reservation> CancelReservation(int reservationId);
        Task<Reservation> CompleteReservation(int reservationId);
        Task ChangeTable(ChangeReservationTablesRequest changeRequest);
        Task<ICollection<TableCurrentStateCacheData>> GetTablesStates(TableStatesRequests getStateRequest);
    }
}
