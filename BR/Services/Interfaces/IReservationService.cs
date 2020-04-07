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

        Task<ServerResponse<string>> SetPendingTableState(TableStatesRequest stateRequest);

        Task<ServerResponse<Reservation>> AddNewReservation(NewReservationRequest newReservationRequest, string identityId);

        Task<ServerResponse> SendReservationOnConfirmation(TableStatesRequest tableStateRequest, NewReservationRequest reservationRequest, int userId, Client client);

        Task<ServerResponse> AddConfirmedReservation(ConfirmReservationRequest confirmRequest);

        Task<ServerResponse> AddReservationByPhone(NewReservationByPhoneRequest reservationRequest, string waiterIdentityId);

        Task<ServerResponse> CancelReservation(int reservationId, int reasonId, string cancelledByIdentityUserId);

        Task<ServerResponse> CompleteReservation(int reservationId);

        Task<ServerResponse> ChangeTable(ChangeReservationTablesRequest changeRequest);

        Task<ICollection<TableCurrentStateCacheData>> GetTablesStates(TableStatesRequest getStateRequest);

        Task<ServerResponse<string>> SetBarPendingTableState(BarStatesRequest stateRequest);

        Task<ServerResponse<Reservation>> AddNewBarReservation(NewBarReservationRequest newReservationRequest, string identityId);

        Task<ServerResponse> SendBarReservationOnConfirmation(BarStatesRequest barStateRequest, NewBarReservationRequest reservationRequest, int userId, Client client);

        Task<ServerResponse> AddBarConfirmedReservation(ConfirmBarReservationRequest confirmRequest);

        Task<ServerResponse> AddBarReservationByPhone(NewBarReservationByPhoneRequest reservationRequest);

        Task<ICollection<BarCurrentStateCacheData>> GetBarTablesStates(BarStatesRequest getStateRequest);

        Task<ServerResponse> AddNewVisitor(NewVisitRequest visitRequest, string addedByIdentityId);

        Task<ServerResponse> CompleteVisit(int visitId);
    }
}
