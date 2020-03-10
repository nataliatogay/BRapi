﻿using BR.DTO;
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
        Task<Reservation> CancelReservation(int reservationId);
        Task<Reservation> CompleteReservation(int reservationId);
        Task<ServerResponse> ChangeTable(ChangeReservationTablesRequest changeRequest);
        Task<ICollection<TableCurrentStateCacheData>> GetTablesStates(TableStatesRequest getStateRequest);
    }
}
