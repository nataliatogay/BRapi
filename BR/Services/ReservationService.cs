﻿using BR.DTO;
using BR.EF;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IAsyncRepository _repository;
        public ReservationService(IAsyncRepository repository)
        {
            _repository = repository;
        }
        public async Task AddNewReservation(NewReservationRequest newReservationRequest, string identityId)
        {
            var user = await _repository.GetUser(identityId);
            var resState = await _repository.GetReservationState("idle");
            var reservation = new Reservation()
            {
                UserId = user.Id,
                ChildFree = newReservationRequest.IsChildFree,
                GuestCount = newReservationRequest.GuestCount,
                Comments = newReservationRequest.Comments,
                ReservationDate = DateTime.ParseExact(newReservationRequest.ReservationDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ReservationStateId = resState.Id
            };
            reservation = await _repository.AddReservation(reservation);

            foreach (var tableId in newReservationRequest.TableIds)
            {
                await _repository.AddTableReservation(reservation.Id, tableId);
            }
        }

        public async Task<Reservation> CancelReservation(int reservationId)
        {
            var resState = await _repository.GetReservationState("cancelled");
            var reservation = await _repository.GetReservation(reservationId);
            reservation.ReservationStateId = resState.Id;
            return await _repository.UpdateReservation(reservation); 

        }

        public async Task<Reservation> CompleteReservation(int reservationId)
        {
            var resState = await _repository.GetReservationState("completed");
            var reservation = await _repository.GetReservation(reservationId);
            reservation.ReservationStateId = resState.Id;
            return await _repository.UpdateReservation(reservation);
        }

        public async Task<Reservation> ChangeTable(int reservationId, IEnumerable<int> newTableIds)
        {
            //await _repository.DeleteTableReservations(reservationId);

            //foreach (var tableId in newReservationRequest.TableIds)
            //{
            //    await _repository.AddTableReservation(reservation.Id, tableId);
            //}

            return null;
        }
    }
}

//Reservation state:
//-idle; - addNew
//-cancelled; - user
//-completed; - waiter
//-not coming; - timer
