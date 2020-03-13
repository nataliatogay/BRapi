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
using System.Timers;

namespace BR.Services
{
    public class ReservationService : IReservationService
    {
        private readonly IAsyncRepository _repository;
        private readonly INotificationService _notificationService;
        private readonly IDistributedCache _cacheDistributed;
        private readonly IMemoryCache _cacheMemory;


        public ReservationService(IAsyncRepository repository,
            INotificationService notificationService,
            IDistributedCache cacheDistributed,
            IMemoryCache cacheMemory)
        {
            _repository = repository;
            _notificationService = notificationService;
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

        private async Task HandlePendingTimer(string key, Timer timer)
        {
            timer.Stop();
            string json = null;
            _cacheMemory.TryGetValue(key, out json);
            if (json != null)
            {
                TableStatesRequest tableStateRequest = JsonConvert.DeserializeObject<TableStatesRequest>(json);
                if (tableStateRequest != null)
                {
                    await this.RemoveTableStateCacheData(DateTime.ParseExact(tableStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), tableStateRequest.Duration, tableStateRequest.TableIds, false);
                }
                _cacheMemory.Remove(key);
            }


        }

        public async Task<ServerResponse<string>> SetPendingTableState(TableStatesRequest stateRequest)
        {
            var resDate = DateTime.ParseExact(stateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            try
            {
                var res = await this.AddTableStateCacheData(resDate, stateRequest.Duration, stateRequest.TableIds, false);
                if (res)
                {
                    var key = Guid.NewGuid().ToString();
                    int timerTime = 2 * 60 * 1000; // add to appsettings
                    Timer timer = new Timer(timerTime);
                    timer.Elapsed += async (sender, e) => await HandlePendingTimer(key, timer);

                    _cacheMemory.Set(key, JsonConvert.SerializeObject(stateRequest));
                    timer.Start();
                    return new ServerResponse<string>(StatusCode.Ok, key);
                }
                else
                {
                    return new ServerResponse<string>(StatusCode.NotAvailable, null);
                }
            }
            catch
            {
                return new ServerResponse<string>(StatusCode.Error, null);
            }

        }

        public async Task<ServerResponse<Reservation>> AddNewReservation(NewReservationRequest newReservationRequest, string identityId)
        {
            string json = null;
            _cacheMemory.TryGetValue(newReservationRequest.Code, out json);
            if (json != null)
            {
                _cacheMemory.Remove(newReservationRequest.Code);
                var tableState = JsonConvert.DeserializeObject<TableStatesRequest>(json);
                if (tableState != null)
                {
                    var user = await _repository.GetUser(identityId);
                    var client = await _repository.GetClientByTableId(tableState.TableIds.First());
                    var resDate = DateTime.ParseExact(tableState.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
                    

                    if (client.ReserveDurationAvg >= tableState.Duration)
                    {
                        var res = await this.SetConfirmedTableStateCacheData(resDate, tableState.Duration, tableState.TableIds);
                        if (!res)
                        {
                            return new ServerResponse<Reservation>(StatusCode.NotAvailable, null);
                        }
                        var reservation = new Reservation()
                        {
                            UserId = user.Id,
                            ChildFree = newReservationRequest.IsChildFree,
                            GuestCount = newReservationRequest.GuestCount,
                            Comments = newReservationRequest.Comments,
                            ReservationDate = resDate,
                            Duration = tableState.Duration,
                            ReservationStateId = null
                        };
                        try
                        {
                            reservation = await _repository.AddReservation(reservation);
                        }
                        catch
                        {
                            return new ServerResponse<Reservation>(StatusCode.Error, null);
                        }
                        try
                        {
                            foreach (var tableId in tableState.TableIds)
                            {
                                await _repository.AddTableReservation(reservation.Id, tableId);
                            }
                        }
                        catch
                        {
                            return new ServerResponse<Reservation>(StatusCode.Error, null);
                        }



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
                                    _notificationService.SendNotification("New reservation", MobilePlatform.gcm, "string", tags.ToArray());
                                }
                                catch
                                {
                                    return new ServerResponse<Reservation>(StatusCode.SendingNotificationError, reservation);
                                }
                            }
                        }
                        return new ServerResponse<Reservation>(reservation);
                    }
                    else
                    {
                        var result = await SendReservationOnConfirmation(tableState, newReservationRequest, user.Id, client);
                        return new ServerResponse<Reservation>(result.StatusCode, null);
                    }
                }
                else
                {
                    return new ServerResponse<Reservation>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<Reservation>(StatusCode.Expired, null);
            }
        }

        private async Task HandleConfirmationTimer(string key, Timer timer, IEnumerable<Waiter> waiters)
        {
            timer.Stop();
            string json = null;
            _cacheMemory.TryGetValue(key, out json);
            if (json != null)
            {
                ConfirmReservationRequest confirmRequest = JsonConvert.DeserializeObject<ConfirmReservationRequest>(json);
                if (confirmRequest != null)
                {
                    await this.RemoveTableStateCacheData(DateTime.ParseExact(confirmRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), confirmRequest.Duration, confirmRequest.TableIds, false);
                }
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
                        _notificationService.SendNotification("Expired", MobilePlatform.gcm, "string", tags.ToArray());
                    }
                    catch { }
                }
                List<string> userTags = new List<string>();
                var accountTokens = await _repository.GetTokens(confirmRequest.UserId);
                if (accountTokens != null)
                {
                    foreach (var token in accountTokens)
                    {
                        userTags.Add(token.NotificationTag);
                    }
                    try
                    {
                        _notificationService.SendNotification("Expired", MobilePlatform.gcm, "string", userTags.ToArray());
                    }
                    catch { }
                }
            }

            _cacheMemory.Remove(key);
        }

        public async Task<ServerResponse> SendReservationOnConfirmation(TableStatesRequest tableStateRequest, NewReservationRequest reservationRequest, int userId, Client client)
        {
            var resDate = DateTime.ParseExact(tableStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var key = Guid.NewGuid().ToString();
            var closeToday = DateTime.Today.AddMinutes(client.CloseTime);
            var waiters = await _repository.GetWaitersByClientId(client.Id);
            DateTime timerTime;
            // а если до конца рабочего дня осталось меньше 15 мин?
            if (closeToday > DateTime.Now)
            {
                timerTime = DateTime.Now.AddMinutes(client.ConfirmationDuration);
            }
            else
            {
                timerTime = DateTime.Today.AddDays(1).AddMinutes(client.OpenTime).AddMinutes(client.ConfirmationDuration);
            }
            Timer timer = new Timer((timerTime - DateTime.Now).TotalMilliseconds);
            timer.Elapsed += async (sender, e) => await HandleConfirmationTimer(key, timer, waiters);
            ConfirmReservationRequest confirmRequest = new ConfirmReservationRequest()
            {
                Acceptance = false,
                Code = key,
                Duration = tableStateRequest.Duration,
                StartDateTime = tableStateRequest.StartDateTime,
                TableIds = tableStateRequest.TableIds,
                IsChildFree = reservationRequest.IsChildFree,
                UserId = userId,
                Comments = reservationRequest.Comments,
                GuestCount = reservationRequest.GuestCount
            };

            _cacheMemory.Set(key, JsonConvert.SerializeObject(confirmRequest));
            timer.Start();

            // уведомлять старшего(-их) официанта(-ов)

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
                    _notificationService.SendNotification("New reservation for confirmation", MobilePlatform.gcm, "string", tags.ToArray());
                    return new ServerResponse(StatusCode.SendOnConfirmation);
                }
                catch
                {
                    return new ServerResponse(StatusCode.SendingNotificationError);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.Error);
            }
        }

        public async Task<ServerResponse> AddConfirmedReservation(ConfirmReservationRequest confirmRequest)
        {
            string json = null;
            _cacheMemory.TryGetValue(confirmRequest.Code, out json);
            if (json is null)
            {
                return new ServerResponse(StatusCode.Expired);
            }
            NewReservationRequest reservationRequest = JsonConvert.DeserializeObject<NewReservationRequest>(json);
            if (reservationRequest is null)
            {
                return new ServerResponse(StatusCode.Error);
            }
            List<string> userTags = new List<string>();
            var accountTokens = await _repository.GetTokens(confirmRequest.UserId);
            if (accountTokens != null)
            {
                foreach (var token in accountTokens)
                {
                    userTags.Add(token.NotificationTag);
                }

            }
            var resDate = DateTime.ParseExact(confirmRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            if (confirmRequest.Acceptance)
            {
                var res = await this.SetConfirmedTableStateCacheData(resDate, confirmRequest.Duration, confirmRequest.TableIds);
                if (!res)
                {
                    return new ServerResponse(StatusCode.NotAvailable);
                }
                var reservation = new Reservation()
                {
                    ChildFree = confirmRequest.IsChildFree,
                    Comments = confirmRequest.Comments,
                    Duration = confirmRequest.Duration,
                    GuestCount = confirmRequest.GuestCount,
                    ReservationDate = resDate,
                    ReservationStateId = null,
                    UserId = confirmRequest.UserId
                };
                reservation = await _repository.AddReservation(reservation);

                foreach (var tableId in confirmRequest.TableIds)
                {
                    await _repository.AddTableReservation(reservation.Id, tableId);
                }

                // notify user
                try
                {
                    _notificationService.SendNotification("reservation accepted", MobilePlatform.gcm, "string", userTags.ToArray());
                }
                catch { }
                return new ServerResponse(StatusCode.Ok);
            }
            else
            {
                await this.RemoveTableStateCacheData(resDate, confirmRequest.Duration, confirmRequest.TableIds, false);
                // notify user
                try
                {
                    _notificationService.SendNotification("reservation rejected", MobilePlatform.gcm, "string", userTags.ToArray());
                }
                catch { }
            }
            _cacheMemory.Remove(confirmRequest.Code);
            return new ServerResponse(StatusCode.Ok);
        }

        public async Task<ServerResponse> AddReservationByPhone(NewReservationByPhoneRequest reservationRequest, string waiterIdentityId)
        {
            var user = await _repository.GetWaiter(waiterIdentityId);
            var client = await _repository.GetClientByTableId(reservationRequest.TableIds.First());
            var resDate = DateTime.ParseExact(reservationRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //var states = await this.GetTablesStates(new TableStatesRequests()
            //{
            //    Duration = reservationRequest.Duration,
            //    StartDateTime = reservationRequest.StartDateTime,
            //    TableIds = reservationRequest.TableIds
            //});
            //if (states != null && states.Count > 0)
            //{
            //    return new ServerResponse<Reservation>(StatusCode.NotAvailable, null);
            //}
            var res = await this.AddTableStateCacheData(resDate, reservationRequest.Duration, reservationRequest.TableIds, true);
            if (!res)
            {
                return new ServerResponse(StatusCode.NotAvailable);
            }
            var reservation = new Reservation()
            {
                ChildFree = reservationRequest.IsChildFree,
                Duration = reservationRequest.Duration,
                GuestCount = reservationRequest.GuestCount,
                ReservationDate = resDate,
                ReservationStateId = null,
                AdditionalInfo = reservationRequest.PhoneNumber + " " + reservationRequest.UserName,
                Comments = reservationRequest.Comments,
            };
            try
            {
                reservation = await _repository.AddReservation(reservation);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }
            try
            {
                foreach (var tableId in reservationRequest.TableIds)
                {
                    await _repository.AddTableReservation(reservation.Id, tableId);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }

            return new ServerResponse(StatusCode.Ok);
        }

        public async Task<ServerResponse> CancelReservation(int reservationId, int reasonId, string cancelledByIdentityUserId)
        {
            var resState = await _repository.GetReservationState("cancelled");
            var cancelReason = await _repository.GetCancelReason(reasonId);
            var reservation = await _repository.GetReservation(reservationId);
            if (resState is null || cancelReason is null || reservation is null)
            {
                return new ServerResponse(StatusCode.Error);
            }
            if (reservation.ReservationDate < DateTime.Now)
            {
                return new ServerResponse(StatusCode.Expired);
            }
            if (reservation.ReservationState != null)
            {
                return new ServerResponse(StatusCode.Error);
            }
            reservation.ReservationStateId = resState.Id;
            reservation.CancelReasonId = reasonId;
            reservation.CancelledByIdentityUserId = cancelledByIdentityUserId;
            ICollection<int> tableIds = new List<int>();
            foreach (var table in reservation.TableReservations)
            {
                tableIds.Add(table.TableId);
            }

            // remove redis data
            await this.RemoveTableStateCacheData(reservation.ReservationDate, reservation.Duration, tableIds, true);
            try
            {
                await _repository.UpdateReservation(reservation);
                return new ServerResponse(StatusCode.Ok);
            }
            catch
            {
                return new ServerResponse(StatusCode.Error);
            }

        }

        public async Task<Reservation> CompleteReservation(int reservationId)
        {
            var resState = await _repository.GetReservationState("completed");
            var reservation = await _repository.GetReservation(reservationId);
            reservation.ReservationStateId = resState.Id;
            return await _repository.UpdateReservation(reservation);
        }

        public async Task<ServerResponse> ChangeTable(ChangeReservationTablesRequest changeRequest)
        {
            var reservation = await _repository.GetReservation(changeRequest.ReservationId);
            if (reservation is null)
            {
                return new ServerResponse(StatusCode.Error);
            }
            ICollection<int> tableIdsPrev = new List<int>();
            foreach (var table in reservation.TableReservations)
            {
                tableIdsPrev.Add(table.TableId);
            }

            // change redis data
            var res = await this.ChangeTableStateCacheData(reservation.ReservationDate, reservation.ReservationDate, reservation.Duration, reservation.Duration, tableIdsPrev, changeRequest.TableIds, true);
            if (!res)
            {
                return new ServerResponse(StatusCode.NotAvailable);
            }
            await _repository.DeleteTableReservations(changeRequest.ReservationId);

            foreach (var tableId in changeRequest.TableIds)
            {
                await _repository.AddTableReservation(changeRequest.ReservationId, tableId);
            }
            return new ServerResponse(StatusCode.Ok);


        }

        public async Task<ICollection<TableCurrentStateCacheData>> GetTablesStates(TableStatesRequest getStateRequest)
        {
            var interval = 15;  // add to appsettings
            ICollection<TableCurrentStateCacheData> tableStates = null;
            var json = await _cacheDistributed.GetStringAsync("tableStates");
            if (json is null)
            {
                return null;
            }

            tableStates = JsonConvert.DeserializeObject<ICollection<TableCurrentStateCacheData>>(json);
            if (tableStates is null)
            {
                return null;
            }

            var res = new List<TableCurrentStateCacheData>();
            var resDate = DateTime.ParseExact(getStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            for (int i = 0; i < getStateRequest.Duration; i += interval)
            {
                foreach (var id in getStateRequest.TableIds)
                {

                    var tStateCache = tableStates.FirstOrDefault(s => s.DateTime.Equals(resDate.AddMinutes(interval)) && s.TableId == id);
                    if (tStateCache != null)
                    {
                        res.Add(tStateCache);
                    }
                }
            }

            return res;
        }


        private async Task<bool> AddTableStateCacheData(DateTime timeStart, int duration, ICollection<int> tableIds, bool isConfirmed)
        {
            var interval = 15;  // add to appsettings
            var json = await _cacheDistributed.GetStringAsync("tableStates");
            ICollection<TableCurrentStateCacheData> tableStates;
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
                    var tableState = tableStates.FirstOrDefault(t => t.TableId == tableId && t.DateTime == timeStart.AddMinutes(i));
                    if (tableState is null)
                    {
                        tableStates.Add(new TableCurrentStateCacheData()
                        {
                            TableId = tableId,
                            DateTime = timeStart.AddMinutes(i),
                            IsConfirmed = isConfirmed
                        });
                    }
                    else
                    {
                        return false;
                    }
                }
            }

            json = JsonConvert.SerializeObject(tableStates);
            await _cacheDistributed.SetStringAsync("tableStates", json);
            return true;
        }

        private async Task<bool> SetConfirmedTableStateCacheData(DateTime timeStart, int duration, ICollection<int> tableIds)
        {
            var interval = 15;  // add to appsettings

            var json = await _cacheDistributed.GetStringAsync("tableStates");
            ICollection<TableCurrentStateCacheData> tableStates;
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
                    var tableState = tableStates.FirstOrDefault(t => t.TableId == tableId && t.DateTime == timeStart.AddMinutes(i));
                    if (tableState is null)
                    {
                        tableStates.Add(new TableCurrentStateCacheData()
                        {
                            TableId = tableId,
                            DateTime = timeStart.AddMinutes(i),
                            IsConfirmed = true
                        });
                    }
                    else if (!tableState.IsConfirmed)
                    {

                        tableState.IsConfirmed = true;
                    }
                    else if (tableState.IsConfirmed)
                    {
                        return false;
                    }
                }
            }

            json = JsonConvert.SerializeObject(tableStates);
            await _cacheDistributed.SetStringAsync("tableStates", json);
            return true;
        }
        private async Task RemoveTableStateCacheData(DateTime timeStart, int duration, ICollection<int> tableIds, bool isConfirmed)
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


                    //if (tableState != null && tableState.IsConfirmed)
                    //{
                    //    if (isConfirmed)
                    //    {
                    //        tableStates.Remove(tableState);
                    //    }
                    //}
                    //else if (tableState != null && !tableState.IsConfirmed)
                    //{
                    //    if (!isConfirmed)
                    //    {
                    //        tableStates.Remove(tableState);
                    //    }
                    //}
                }
            }

            json = JsonConvert.SerializeObject(tableStates);

            await _cacheDistributed.SetStringAsync("tableStates", json);
        }

        private async Task<bool> ChangeTableStateCacheData(DateTime timePrev, DateTime timeNew, int durationPrev, int durationNew, ICollection<int> tableIdsPrev, ICollection<int> tableIdsNew, bool isConfirmed)
        {
            var interval = 15;  // add to appsettings
            var json = await _cacheDistributed.GetStringAsync("tableStates");
            if (json is null)
            {
                //?
                return false;
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
                    var tableState = tableStates.FirstOrDefault(t => t.TableId == tableId && t.DateTime == timePrev.AddMinutes(i));
                    if (tableState != null)
                    {
                        return false;
                    }
                    tableStates.Add(new TableCurrentStateCacheData()
                    {
                        TableId = tableId,
                        IsConfirmed = isConfirmed,
                        DateTime = timeNew.AddMinutes(i)
                    });
                }
            }

            json = JsonConvert.SerializeObject(tableStates);

            await _cacheDistributed.SetStringAsync("tableStates", json);
            return true;
        }
    }
}

//Reservation state:
//-idle; - addNew
//-cancelled; - user, waiter
//-completed; - waiter
//-missed; - timer; 
