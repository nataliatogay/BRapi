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

        Task<ServerResponse<ICollection<ReservationInfoForClient>>> GetReservationsByClient(string fromDate, string toDate, string clientIdentityId);

        Task<ServerResponse<string>> SetPendingTableState(TableState tableState);

        Task<ServerResponse> AddNewReservationByUser(NewReservationByUserRequest newReservationRequest, string userIdentityId);

        Task<ServerResponse> AddConfirmedReservation(ConfirmReservationRequest confirmRequest, string clientIdentityId);









        //===========================================================================







        Task<ICollection<Reservation>> GetReservations(string identityUserId);

        Task<Reservation> GetReservation(int id);





        //Task<ServerResponse> SendReservationOnConfirmation(TableState tableStateRequest, NewReservationRequest reservationRequest, int userId, Client client);


        Task<ServerResponse> AddReservationByPhone(NewReservationByPhoneRequest reservationRequest, string waiterIdentityId);

        Task<ServerResponse> CancelReservation(int reservationId, int reasonId, string cancelledByIdentityUserId);

        Task<ServerResponse> CompleteReservation(int reservationId);

        Task<ServerResponse> ChangeTable(ChangeReservationTablesRequest changeRequest);

        Task<ICollection<TableCurrentStateCacheData>> GetTablesStates(TableState getStateRequest);

        Task<ServerResponse<string>> SetBarPendingTableState(BarStates stateRequest);

        Task<ServerResponse<Reservation>> AddNewBarReservation(NewBarReservationRequest newReservationRequest, string identityId);

        Task<ServerResponse> SendBarReservationOnConfirmation(BarStates barStateRequest, NewBarReservationRequest reservationRequest, int userId, Client client);

        Task<ServerResponse> AddBarConfirmedReservation(ConfirmBarReservationRequest confirmRequest);

        Task<ServerResponse> AddBarReservationByPhone(NewBarReservationByPhoneRequest reservationRequest);

        Task<ICollection<BarCurrentStateCacheData>> GetBarStates(BarStates getStateRequest);

        Task<ServerResponse> AddNewVisitor(NewVisitRequest visitRequest, string addedByIdentityId);

        Task<ServerResponse> CompleteVisit(int visitId);
    }
}
