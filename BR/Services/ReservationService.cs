using BR.DTO;
using BR.DTO.Reservations;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils.Notification;
using Microsoft.Extensions.Caching.Distributed;
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
        private readonly INotificationService _notificatinoService;
        private readonly IDistributedCache _cache;

        public ReservationService(IAsyncRepository repository, 
            INotificationService notificationService,
            IDistributedCache cache)
        {
            _repository = repository;
            _notificatinoService = notificationService;
            _cache = cache;
        }

        public async Task<ICollection<Reservation>> GetReservations(string identityUserId)
        {
            var user = _repository.GetUser(identityUserId);
            if (user is null)
            {
                return null;
            }
            return await _repository.GetReservations(user.Id);
        }

        public async Task<Reservation> GetReservation(int id)
        {
            return await _repository.GetReservation(id);
        }
        public async Task<Reservation> AddNewReservation(NewReservationRequest newReservationRequest, string identityId)
        {
            var user = await _repository.GetUser(identityId);
            //var resState = await _repository.GetReservationState("idle");
            var reservation = new Reservation()
            {
                UserId = user.Id,
                ChildFree = newReservationRequest.IsChildFree,
                GuestCount = newReservationRequest.GuestCount,
                Comments = newReservationRequest.Comments,
                ReservationDate = DateTime.ParseExact(newReservationRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture),
                ReservationStateId = null
            };
            reservation = await _repository.AddReservation(reservation);

            foreach (var tableId in newReservationRequest.TableIds)
            {
                await _repository.AddTableReservation(reservation.Id, tableId);
            }
            var client = _repository.GetClientByTableId(newReservationRequest.TableIds.First());
            if (client != null)
            {
                var waiters = await _repository.GetWaitersByClientId(client.Id);
                if (waiters != null)
                {
                    List<string> tags = new List<string>();
                    foreach (var waiter in waiters)
                    {
                        var tokens = await _repository.GetTokens(waiter.IdentityId);
                        foreach (var t in tokens)
                        {
                            tags.Add(t.NotificationTag); 
                        }
                    }
                    _notificatinoService.SendNotification("New reservation", MobilePlatform.gcm, "string", tags.ToArray());
                }
            }
            return reservation;
        }

        public async Task<Reservation> CancelReservation(int reservationId)
        {
            var resState = await _repository.GetReservationState("cancelled");
            var reservation = await _repository.GetReservation(reservationId);
            if (reservation.ReservationDate < DateTime.Now)
            {
                return null;
            }
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

        public async Task ChangeTable(ChangeReservationTablesRequest changeRequest)
        {
            await _repository.DeleteTableReservations(changeRequest.ReservationId);

            foreach (var tableId in changeRequest.TableIds)
            {
                await _repository.AddTableReservation(changeRequest.ReservationId, tableId);
            }
        }
    }
}

//Reservation state:
//-idle; - addNew
//-cancelled; - user
//-completed; - waiter
//-missed; - timer; 
