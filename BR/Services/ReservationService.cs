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
            var user = _repository.GetUser(identityId);
            var reservation = new Reservation()
            {
                UserId = user.Id,
                ChildFree = newReservationRequest.IsChildFree,
                GuestCount = newReservationRequest.GuestCount,
                Comments = newReservationRequest.Comments,
                ReservationDate = DateTime.ParseExact(newReservationRequest.ReservationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture) // change format
            };
           // reservation.ReservationStateId = _repository.GetReservationState("idle");
            reservation = await _repository.AddNewReservation(reservation);

            foreach (var tableId in newReservationRequest.TableIds)
            {
                await _repository.AddTableReservation(reservation.Id, tableId);
            }
        }

        public async Task CancelReservation(int reservationId)
        {


        }

        public async Task CompleteReservation(int reservationId)
        {

        }

        public async Task ChangeTable(int reservationId, IEnumerable<int> tableIds)
        {

        }
    }
}

//Reservation state:
//-idle; - addNew
//-canceled; - user
//-completed; - waiter
//-not coming; - timer
