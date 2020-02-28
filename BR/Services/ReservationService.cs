using BR.DTO;
using BR.DTO.Redis;
using BR.DTO.Reservations;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using BR.Utils.Notification;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BR.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IAsyncRepository _repository;
        private readonly INotificationService _notificatinoService;
        private readonly IDistributedCache _cacheDistributed;
        private readonly IMemoryCache _cacheMemory;


        public ReservationService(IAsyncRepository repository,
            INotificationService notificationService,
            IDistributedCache cacheDistributed,
            IMemoryCache cacheMemory)
        {
            _repository = repository;
            _notificatinoService = notificationService;
            _cacheDistributed = cacheDistributed;
            _cacheMemory = cacheMemory;
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


        public async Task<ServiceResponse<Reservation>> AddNewReservation(NewReservationRequest newReservationRequest, string identityId)
        {
            var user = await _repository.GetUser(identityId);
            var client = await _repository.GetClientByTableId(newReservationRequest.TableIds.First());
            var resDate = DateTime.ParseExact(newReservationRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            if (client.ReserveDurationAvg >= newReservationRequest.Duration)
            {

                var reservation = new Reservation()
                {
                    UserId = user.Id,
                    ChildFree = newReservationRequest.IsChildFree,
                    GuestCount = newReservationRequest.GuestCount,
                    Comments = newReservationRequest.Comments,
                    ReservationDate = resDate,
                    Duration = newReservationRequest.Duration,
                    ReservationStateId = null
                };
                reservation = await _repository.AddReservation(reservation);

                foreach (var tableId in newReservationRequest.TableIds)
                {
                    await _repository.AddTableReservation(reservation.Id, tableId);
                }


                // add data to redis
                await this.AddTableStateCacheData(resDate, newReservationRequest.Duration, newReservationRequest.TableIds);

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
                        try
                        {
                            _notificatinoService.SendNotification("New reservation", MobilePlatform.gcm, "string", tags.ToArray());

                        }
                        catch
                        {
                            return new ServiceResponse<Reservation>(StatusCodeService.SendingNotificationError, reservation);
                        }
                    }
                }
                return new ServiceResponse<Reservation>(reservation);
            }
            else
            {
                return await SendReservationOnConfirmation(newReservationRequest, user.Id, client);
            }
        }

        class ReservationConfirm
        {
            public int UserId { get; set; }
            public string StartDaterTime { get; set; }
            public int Duration { get; set; }
            public ICollection<int> TableIds { get; set; }
        }

        public async Task<ServiceResponse<Reservation>> SendReservationOnConfirmation(NewReservationRequest newReservationRequest, int userId, Client client)
        {
            
            // time for cache -> to appsettings


            _cacheMemory.Set(userId, JsonConvert.SerializeObject(newReservationRequest), TimeSpan.FromMinutes(60));


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
                try
                {
                    _notificatinoService.SendNotification("New reservation for confirmation", MobilePlatform.gcm, "string", tags.ToArray());
                    return new ServiceResponse<Reservation>(StatusCodeService.SendOnConfirmation, null);
                }
                catch
                {
                    return new ServiceResponse<Reservation>(StatusCodeService.SendingNotificationError, null);
                }
            }
            else
            {
                return new ServiceResponse<Reservation>(StatusCodeService.Error, null);
            }
        }

        public async Task<ServiceResponse<Reservation>> AddConfirmedReservation(ConfirmReservationRequest confirmRequest)
        {

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
            ICollection<int> tableIds = new List<int>();
            foreach (var table in reservation.TableReservations)
            {
                tableIds.Add(table.TableId);
            }

            // remove redis data
            await this.RemoveTableStateCacheData(reservation.ReservationDate, reservation.Duration, tableIds);
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
            var reservation = await _repository.GetReservation(changeRequest.ReservationId);
            if (reservation is null)
            {
                return;
            }
            ICollection<int> tableIdsPrev = new List<int>();
            foreach (var table in reservation.TableReservations)
            {
                tableIdsPrev.Add(table.TableId);
            }
            await _repository.DeleteTableReservations(changeRequest.ReservationId);

            foreach (var tableId in changeRequest.TableIds)
            {
                await _repository.AddTableReservation(changeRequest.ReservationId, tableId);
            }

            // change redis data

            await this.ChangeTableStateCacheData(reservation.ReservationDate, reservation.ReservationDate, reservation.Duration, reservation.Duration, tableIdsPrev, changeRequest.TableIds);


        }



        private async Task AddTableStateCacheData(DateTime timeStart, int duration, ICollection<int> tableIds)
        {
            var interval = 15;  // add to appsettings
            ICollection<TableCurrentStateCacheData> tableStates = null;
            var json = await _cacheDistributed.GetStringAsync("tableStates");
            if (json != null)
            {
                tableStates = JsonConvert.DeserializeObject<ICollection<TableCurrentStateCacheData>>(json);
            }
            else
            {
                tableStates = new List<TableCurrentStateCacheData>();

            }
            foreach (var tableId in tableIds)
            {

                for (int i = 0; i < duration; i += interval)
                {
                    tableStates.Add(new TableCurrentStateCacheData()
                    {
                        TableId = tableId,
                        DateTime = timeStart.AddMinutes(i)
                    });
                }
            }

            json = JsonConvert.SerializeObject(tableStates);

            await _cacheDistributed.SetStringAsync("tableStates", json);
        }

        private async Task RemoveTableStateCacheData(DateTime timeStart, int duration, ICollection<int> tableIds)
        {
            var interval = 15;  // add to appsettings

            var json = await _cacheDistributed.GetStringAsync("tableStates");

            if (json is null)
            {
                return;
            }
            ICollection<TableCurrentStateCacheData> tableStates = JsonConvert.DeserializeObject<ICollection<TableCurrentStateCacheData>>(json);

            foreach (var tableId in tableIds)
            {
                for (int i = 0; i < duration; i += interval)
                {
                    var tableState = tableStates.FirstOrDefault(t => t.TableId == tableId && t.DateTime == timeStart.AddMinutes(i));
                    if (tableState != null)
                    {
                        tableStates.Remove(tableState);
                    }
                }
            }

            json = JsonConvert.SerializeObject(tableStates);

            await _cacheDistributed.SetStringAsync("tableStates", json);
        }

        private async Task ChangeTableStateCacheData(DateTime timePrev, DateTime timeNew, int durationPrev, int durationNew, ICollection<int> tableIdsPrev, ICollection<int> tableIdsNew)
        {
            var interval = 15;  // add to appsettings
            var json = await _cacheDistributed.GetStringAsync("tableStates");
            if (json is null)
            {
                return;
            }
            ICollection<TableCurrentStateCacheData> tableStates = JsonConvert.DeserializeObject<ICollection<TableCurrentStateCacheData>>(json);

            // delete previous states
            foreach (var tableId in tableIdsPrev)
            {
                for (int i = 0; i < durationPrev; i += interval)
                {
                    var tableState = tableStates.FirstOrDefault(t => t.TableId == tableId && t.DateTime == timePrev.AddMinutes(i));
                    if (tableState != null)
                    {
                        tableStates.Remove(tableState);
                    }
                }
            }

            // add new states
            foreach (var tableId in tableIdsNew)
            {

                for (int i = 0; i < durationNew; i += interval)
                {
                    tableStates.Add(new TableCurrentStateCacheData()
                    {
                        TableId = tableId,
                        DateTime = timeNew.AddMinutes(i)
                    });
                }
            }

            json = JsonConvert.SerializeObject(tableStates);

            await _cacheDistributed.SetStringAsync("tableStates", json);
        }
    }
}

//Reservation state:
//-idle; - addNew
//-cancelled; - user
//-completed; - waiter
//-missed; - timer; 
