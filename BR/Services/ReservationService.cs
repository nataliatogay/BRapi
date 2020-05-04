using BR.DTO;
using BR.DTO.Redis;
using BR.DTO.Reservations;
using BR.DTO.Schema;
using BR.DTO.Users;
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
        private readonly IPushNotificationService _notificationService;
        private readonly IDistributedCache _cacheDistributed;
        private readonly IMemoryCache _cacheMemory;


        public ReservationService(IAsyncRepository repository,
            IPushNotificationService notificationService,
            IDistributedCache cacheDistributed,
            IMemoryCache cacheMemory)
        {
            _repository = repository;
            _notificationService = notificationService;
            _cacheDistributed = cacheDistributed;
            _cacheMemory = cacheMemory;
        }



        public async Task<ServerResponse<ICollection<ReservationInfoForClient>>> GetReservationsByClient(string fromDate, string toDate, string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.DbConnectionError, null);
            }
            var fromDateDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var toDateDate = DateTime.ParseExact(toDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var reservations = client.Reservations;
            var response = new List<ReservationInfoForClient>();
            foreach (var item in reservations)
            {
                if (item.ReservationDate >= fromDateDate && item.ReservationDate <= toDateDate)
                {
                    var user = await _repository.GetUser(item.IdentityUserId);
                    var invitees = new List<UserFullInfoForClient>();
                    foreach (var inv in item.Invitees)
                    {
                        invitees.Add(UserToUserFullInfoForClient(inv.User));
                    }
                    response.Add(
                        new ReservationInfoForClient()
                        {
                            Id = item.Id,
                            ChildFree = item.ChildFree,
                            Comments = item.Comments,
                            StartDateTime = item.ReservationDate,
                            EndDateTime = item.ReservationDate.AddMinutes(item.Duration),
                            GuestCount = item.GuestCount,
                            Invalids = item.Invalids,
                            Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                            PetsFree = item.PetsFree,
                            State = item.ReservationState is null ? "idle" : item.ReservationState.Title,
                            User = UserToUserFullInfoForClient(user),
                            ApplicationDate = item.ReservationRequest.IssueDate,
                            Invitees = invitees
                        });
                }
            }
            return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.Ok, response);
        }


        public async Task<ServerResponse<ICollection<ReservationInfoForClient>>> GetAllReservationsByClient(string clientIdentityId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityId);
                if (client is null)
                {
                    return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.DbConnectionError, null);
            }
            var reservations = client.Reservations;
            var response = new List<ReservationInfoForClient>();
            foreach (var item in reservations)
            {
                var user = await _repository.GetUser(item.IdentityUserId);
                var invitees = new List<UserFullInfoForClient>();
                foreach (var inv in item.Invitees)
                {
                    invitees.Add(UserToUserFullInfoForClient(inv.User));
                }
                response.Add(
                    new ReservationInfoForClient()
                    {
                        Id = item.Id,
                        ChildFree = item.ChildFree,
                        Comments = item.Comments,
                        StartDateTime = item.ReservationDate,
                        EndDateTime = item.ReservationDate.AddMinutes(item.Duration),
                        GuestCount = item.GuestCount,
                        Invalids = item.Invalids,
                        Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                        PetsFree = item.PetsFree,
                        State = item.ReservationState is null ? "idle" : item.ReservationState.Title,
                        ApplicationDate = item.ReservationRequest.IssueDate,
                        User = UserToUserFullInfoForClient(user),
                        Invitees = invitees
                    });
            }
            return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.Ok, response);
        }


        public async Task<ServerResponse<ICollection<ReservationRequestInfoForClient>>> GetReservationRequestsForClient(string clientIdentityUserId)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityUserId);

                if (client is null)
                {
                    return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.DbConnectionError, null);
            }

            var requests = await _repository.GetWaitingReservationRequestsByClientId(client.Id);

            var res = new List<ReservationRequestInfoForClient>();
            foreach (var item in requests)
            {
                var user = await _repository.GetUser(item.RequestedByIdentityId);
                List<int> inviteeIds = JsonConvert.DeserializeObject<List<int>>(item.InviteeIds);
                var invitees = new List<UserFullInfoForClient>();
                foreach (var inv in inviteeIds)
                {
                    invitees.Add(this.UserToUserFullInfoForClient(await _repository.GetUser(inv)));
                }
                res.Add(
                    new ReservationRequestInfoForClient()
                    {
                        Id = item.Id,
                        ChildFree = item.ChildFree,
                        Comments = item.Comments,
                        StartDateTime = item.ReservationDateTime,
                        EndDateTime = item.ReservationDateTime.AddMinutes(item.Duration),
                        GuestCount = item.GuestCount,
                        Invalids = item.Invalids,
                        PetsFree = item.PetsFree,
                        Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                        IssueDate = item.IssueDate,
                        User = this.UserToUserFullInfoForClient(await _repository.GetUser(item.RequestedByIdentityId)),
                        Invitees = invitees,
                        State = item.ReservationRequestStateId is null ? "idle" : item.ReservationRequestState.Title
                    });
            }
            return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.Ok, res);

        }


        public async Task<ServerResponse<ICollection<ReservationRequestInfoForClient>>> GetRejectedReservationRequestsForClient(string clientIdentityUserId, string date)
        {
            Client client;
            try
            {
                client = await _repository.GetClient(clientIdentityUserId);

                if (client is null)
                {
                    return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.UserNotFound, null);
                }
            }
            catch
            {
                return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.DbConnectionError, null);
            }

            var resDate = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var requests = await _repository.GetRejectedReservationRequestsByClientId(client.Id, resDate);

            var res = new List<ReservationRequestInfoForClient>();
            foreach (var item in requests)
            {
                var user = await _repository.GetUser(item.RequestedByIdentityId);
                List<int> inviteeIds = JsonConvert.DeserializeObject<List<int>>(item.InviteeIds);
                var invitees = new List<UserFullInfoForClient>();
                foreach (var inv in inviteeIds)
                {
                    invitees.Add(this.UserToUserFullInfoForClient(await _repository.GetUser(inv)));
                }
                res.Add(
                    new ReservationRequestInfoForClient()
                    {
                        Id = item.Id,
                        ChildFree = item.ChildFree,
                        Comments = item.Comments,
                        StartDateTime = item.ReservationDateTime,
                        EndDateTime = item.ReservationDateTime.AddMinutes(item.Duration),
                        GuestCount = item.GuestCount,
                        Invalids = item.Invalids,
                        PetsFree = item.PetsFree,
                        Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                        IssueDate = item.IssueDate,
                        User = this.UserToUserFullInfoForClient(await _repository.GetUser(item.RequestedByIdentityId)),
                        Invitees = invitees,
                        State = item.ReservationRequestStateId is null ? "idle" : item.ReservationRequestState.Title
                    });
            }
            return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.Ok, res);

        }


        public async Task<ServerResponse<ICollection<ReservationInfoForClient>>> GetReservationsByOwner(string fromDate, string toDate, int clientId, string ownerIdentityId)
        {


            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.DbConnectionError, null);
            }


            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.NotFound, null);
            }
            var fromDateDate = DateTime.ParseExact(fromDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var toDateDate = DateTime.ParseExact(toDate, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var reservations = client.Reservations;
            var response = new List<ReservationInfoForClient>();
            foreach (var item in reservations)
            {
                if (item.ReservationDate >= fromDateDate && item.ReservationDate <= toDateDate)
                {
                    var user = await _repository.GetUser(item.IdentityUserId);
                    var invitees = new List<UserFullInfoForClient>();
                    foreach (var inv in item.Invitees)
                    {
                        invitees.Add(UserToUserFullInfoForClient(inv.User));
                    }
                    response.Add(
                        new ReservationInfoForClient()
                        {
                            Id = item.Id,
                            ChildFree = item.ChildFree,
                            Comments = item.Comments,
                            StartDateTime = item.ReservationDate,
                            EndDateTime = item.ReservationDate.AddMinutes(item.Duration),
                            GuestCount = item.GuestCount,
                            Invalids = item.Invalids,
                            Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                            PetsFree = item.PetsFree,
                            State = item.ReservationState is null ? "idle" : item.ReservationState.Title,
                            ApplicationDate = item.ReservationRequest.IssueDate,
                            User = UserToUserFullInfoForClient(user),
                            Invitees = invitees
                        });
                }
            }
            return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.Ok, response);
        }


        public async Task<ServerResponse<ICollection<ReservationInfoForClient>>> GetAllReservationsByOwner(int clientId, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.DbConnectionError, null);
            }


            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.NotFound, null);
            }

            var reservations = client.Reservations;
            var response = new List<ReservationInfoForClient>();
            foreach (var item in reservations)
            {
                var user = await _repository.GetUser(item.IdentityUserId);
                var invitees = new List<UserFullInfoForClient>();
                foreach (var inv in item.Invitees)
                {
                    invitees.Add(UserToUserFullInfoForClient(inv.User));
                }
                response.Add(
                    new ReservationInfoForClient()
                    {
                        Id = item.Id,
                        ChildFree = item.ChildFree,
                        Comments = item.Comments,
                        StartDateTime = item.ReservationDate,
                        EndDateTime = item.ReservationDate.AddMinutes(item.Duration),
                        GuestCount = item.GuestCount,
                        Invalids = item.Invalids,
                        Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                        PetsFree = item.PetsFree,
                        State = item.ReservationState is null ? "idle" : item.ReservationState.Title,
                        ApplicationDate = item.ReservationRequest.IssueDate,
                        User = UserToUserFullInfoForClient(user),
                        Invitees = invitees
                    });
            }
            return new ServerResponse<ICollection<ReservationInfoForClient>>(StatusCode.Ok, response);
        }


        public async Task<ServerResponse<ICollection<ReservationRequestInfoForClient>>> GetReservationRequestsForOwner(string ownerIdentityId, int clientId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.DbConnectionError, null);
            }


            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.NotFound, null);
            }

            var requests = await _repository.GetWaitingReservationRequestsByClientId(client.Id);

            var res = new List<ReservationRequestInfoForClient>();
            foreach (var item in requests)
            {
                var user = await _repository.GetUser(item.RequestedByIdentityId);
                List<int> inviteeIds = JsonConvert.DeserializeObject<List<int>>(item.InviteeIds);
                var invitees = new List<UserFullInfoForClient>();
                foreach (var inv in inviteeIds)
                {
                    invitees.Add(this.UserToUserFullInfoForClient(await _repository.GetUser(inv)));
                }
                res.Add(
                    new ReservationRequestInfoForClient()
                    {
                        Id = item.Id,
                        ChildFree = item.ChildFree,
                        Comments = item.Comments,
                        StartDateTime = item.ReservationDateTime,
                        EndDateTime = item.ReservationDateTime.AddMinutes(item.Duration),
                        GuestCount = item.GuestCount,
                        Invalids = item.Invalids,
                        PetsFree = item.PetsFree,
                        Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                        IssueDate = item.IssueDate,
                        User = this.UserToUserFullInfoForClient(await _repository.GetUser(item.RequestedByIdentityId)),
                        Invitees = invitees,
                        State = item.ReservationRequestStateId is null ? "idle" : item.ReservationRequestState.Title
                    });
            }
            return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.Ok, res);

        }


        public async Task<ServerResponse<ICollection<ReservationRequestInfoForClient>>> GetRejectedReservationRequestsForOwner(string ownerIdentityId, int clientId, string date)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null || owner.Organization is null)
                {
                    return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.UserNotFound, null);
                }

            }
            catch
            {
                return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.DbConnectionError, null);
            }


            Client client = owner.Organization.Clients.FirstOrDefault(item => item.Id == clientId);

            if (client is null)
            {
                return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.NotFound, null);
            }

            var resDate = DateTime.ParseExact(date, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var requests = await _repository.GetRejectedReservationRequestsByClientId(client.Id, resDate);

            var res = new List<ReservationRequestInfoForClient>();
            foreach (var item in requests)
            {
                var user = await _repository.GetUser(item.RequestedByIdentityId);
                List<int> inviteeIds = JsonConvert.DeserializeObject<List<int>>(item.InviteeIds);
                var invitees = new List<UserFullInfoForClient>();
                foreach (var inv in inviteeIds)
                {
                    invitees.Add(this.UserToUserFullInfoForClient(await _repository.GetUser(inv)));
                }
                res.Add(
                    new ReservationRequestInfoForClient()
                    {
                        Id = item.Id,
                        ChildFree = item.ChildFree,
                        Comments = item.Comments,
                        StartDateTime = item.ReservationDateTime,
                        EndDateTime = item.ReservationDateTime.AddMinutes(item.Duration),
                        GuestCount = item.GuestCount,
                        Invalids = item.Invalids,
                        PetsFree = item.PetsFree,
                        Table = new TableInfo() { Id = item.TableId, Number = item.Table.Number },
                        IssueDate = item.IssueDate,
                        User = this.UserToUserFullInfoForClient(await _repository.GetUser(item.RequestedByIdentityId)),
                        Invitees = invitees,
                        State = item.ReservationRequestStateId is null ? "idle" : item.ReservationRequestState.Title
                    });
            }
            return new ServerResponse<ICollection<ReservationRequestInfoForClient>>(StatusCode.Ok, res);

        }


        private async Task HandlePendingTimer(string key, Timer timer)
        {
            //string json = null;
            //_cacheMemory.TryGetValue(key, out json);
            var json = await _cacheDistributed.GetStringAsync(key);
            if (json != null)
            {
                //_cacheMemory.Remove(key);
                await _cacheDistributed.RemoveAsync(key);
                TableState tableStateRequest = JsonConvert.DeserializeObject<TableState>(json);
                if (tableStateRequest != null)
                {
                    await this.RemoveTableStateCacheData(DateTime.ParseExact(tableStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), tableStateRequest.Duration, tableStateRequest.TableId, false);
                }


            }
            timer.Stop();
        }



        public async Task<ServerResponse<string>> SetPendingTableState(TableState tableState)
        {
            var resDate = DateTime.ParseExact(tableState.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            try
            {
                var res = await this.AddTableStateCacheData(resDate, tableState.Duration, tableState.TableId, false);
                if (res)
                {
                    var key = Guid.NewGuid().ToString();
                    int timerTime = 2 * 60 * 1000; // add to appsettings
                    Timer timer = new Timer(timerTime);
                    timer.Elapsed += async (sender, e) => await HandlePendingTimer(key, timer);

                    //_cacheMemory.Set(key, JsonConvert.SerializeObject(tableState));
                    await _cacheDistributed.SetStringAsync(key, JsonConvert.SerializeObject(tableState));
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


        public async Task<ServerResponse> AddNewReservationByUser(NewReservationByUserRequest newReservationRequest, string userIdentityId)
        {
            //string json = null;
            //_cacheMemory.TryGetValue(newReservationRequest.Code, out json);

            var json = await _cacheDistributed.GetStringAsync(newReservationRequest.Code);
            if (json != null)
            {
                //_cacheMemory.Remove(newReservationRequest.Code);
                await _cacheDistributed.RemoveAsync(newReservationRequest.Code);

                var tableState = JsonConvert.DeserializeObject<TableState>(json);
                if (tableState != null)
                {
                    var user = await _repository.GetUser(userIdentityId);

                    var table = await _repository.GetTable(tableState.TableId);

                    if (table is null)
                    {
                        return new ServerResponse(StatusCode.Error);
                    }

                    var client = table.Hall.Floor.Client;

                    var resDate = DateTime.ParseExact(tableState.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

                    //if (client.ReserveDurationAvg >= tableState.Duration)
                    //{
                    //var res = await this.SetConfirmedTableStateCacheData(resDate, tableState.Duration, tableState.TableIds);
                    //if (!res)
                    //{
                    //    return new ServerResponse<Reservation>(StatusCode.NotAvailable, null);
                    //}
                    //var reservation = new Reservation()
                    //{
                    //    UserId = user.Id,
                    //    ChildFree = newReservationRequest.IsChildFree,
                    //    GuestCount = newReservationRequest.GuestCount,
                    //    Comments = newReservationRequest.Comments,
                    //    ReservationDate = resDate,
                    //    Duration = tableState.Duration,
                    //    ReservationStateId = null,
                    //    PetsFree = newReservationRequest.IsPetsFree,
                    //    Invalids = newReservationRequest.Invalids,
                    //    ClientId = client.Id
                    //};
                    //try
                    //{
                    //    reservation = await _repository.AddReservation(reservation);
                    //}
                    //catch
                    //{
                    //    return new ServerResponse<Reservation>(StatusCode.Error, null);
                    //}
                    //try
                    //{
                    //    foreach (var tableId in tableState.TableIds)
                    //    {
                    //        await _repository.AddTableReservation(new TableReservation()
                    //        {
                    //            ReservationId = reservation.Id,
                    //            TableId = tableId
                    //        });
                    //    }
                    //}
                    //catch
                    //{
                    //    return new ServerResponse<Reservation>(StatusCode.Error, null);
                    //}



                    //if (client != null)
                    //{
                    //    var waiters = client.Waiters;
                    //    if (waiters != null)
                    //    {
                    //        List<string> tags = new List<string>();
                    //        foreach (var waiter in waiters)
                    //        {
                    //            var tokens = await _repository.GetTokens(waiter.IdentityId);
                    //            foreach (var t in tokens)
                    //            {
                    //                tags.Add(t.NotificationTag);
                    //            }
                    //        }
                    //        try
                    //        {
                    //            _notificationService.SendNotification("New reservation", MobilePlatform.gcm, "string", tags.ToArray());
                    //        }
                    //        catch
                    //        {
                    //            return new ServerResponse<Reservation>(StatusCode.SendingNotificationError, reservation);
                    //        }
                    //    }
                    //}
                    //return new ServerResponse<Reservation>(reservation);
                    //}
                    //else
                    //{

                    ReservationRequest reservationRequest = new ReservationRequest()
                    {
                        ChildFree = newReservationRequest.IsChildFree,
                        PetsFree = newReservationRequest.IsPetsFree,
                        Invalids = newReservationRequest.Invalids,
                        Comments = newReservationRequest.Comments,
                        Duration = tableState.Duration,
                        InviteeIds = JsonConvert.SerializeObject(newReservationRequest.InviteeIds),
                        GuestCount = newReservationRequest.GuestCount,
                        ReservationDateTime = resDate,
                        TableId = tableState.TableId,
                        RequestedByIdentityId = userIdentityId,
                        IssueDate = DateTime.Now
                    };
                    try
                    {
                        reservationRequest = await _repository.AddReservationRequest(reservationRequest);
                        return await SendReservationOnConfirmation(reservationRequest.Id, client);

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //return new ServerResponse(StatusCode.DbConnectionError);
                    }

                    //}
                }
                else
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.Expired);
            }

        }







        private async Task HandleConfirmationTimer(int reservationRequestId, Timer timer)
        {

            //string json = null;
            //_cacheMemory.TryGetValue("reservationRequests", out json);
            var json = await _cacheDistributed.GetStringAsync("reservationRequests");
            if (json != null)
            {

                ICollection<int> reservationRequestsCache = JsonConvert.DeserializeObject<ICollection<int>>(json);
                if (reservationRequestsCache.Contains(reservationRequestId))
                {
                    reservationRequestsCache.Remove(reservationRequestId);

                    //_cacheMemory.Set("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));
                    await _cacheDistributed.SetStringAsync("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));
                    ReservationRequest resRequest = await _repository.GetReservationRequest(reservationRequestId);

                    if (resRequest != null)
                    {
                        await this.RemoveTableStateCacheData(resRequest.ReservationDateTime, resRequest.Duration, resRequest.TableId, false);
                    }
                    var waiters = resRequest.Table.Hall.Floor.Client.Waiters;
                    if (waiters != null)
                    {

                        // уведомлять старшего официанта

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
                    var accountTokens = await _repository.GetTokens(resRequest.RequestedByIdentityId);
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

            }
            timer.Stop();


        }


        public async Task<ServerResponse> SendReservationOnConfirmation(int reservationRequestId, Client client)
        {
            var closeToday = DateTime.Today.AddMinutes(client.CloseTime);
            var waiters = client.Waiters;
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
            timer.Elapsed += async (sender, e) => await HandleConfirmationTimer(reservationRequestId, timer);

            //string json = null;
            //_cacheMemory.TryGetValue("reservationRequests", out json);
            var json = await _cacheDistributed.GetStringAsync("reservationRequests");

            ICollection<int> reservationRequestsCache;
            if (json is null)
            {
                reservationRequestsCache = new List<int>();
            }
            else
            {
                reservationRequestsCache = JsonConvert.DeserializeObject<ICollection<int>>(json);
            }
            reservationRequestsCache.Add(reservationRequestId);




            //_cacheMemory.Set("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));
            await _cacheDistributed.SetStringAsync("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));
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




        public async Task<ServerResponse> AddConfirmedReservationByClient(ConfirmReservationRequest confirmRequest, string clientIdentityId)
        {
            //string json = null;
            //_cacheMemory.TryGetValue("reservationRequests", out json);
            var json = await _cacheDistributed.GetStringAsync("reservationRequests");
            if (json is null)
            {
                return new ServerResponse(StatusCode.Error);
            }
            ICollection<int> reservationRequestsCache = JsonConvert.DeserializeObject<ICollection<int>>(json);
            if (!reservationRequestsCache.Contains(confirmRequest.ReservationRequestId))
            {
                return new ServerResponse(StatusCode.Expired);
            }

            reservationRequestsCache.Remove(confirmRequest.ReservationRequestId);

            //_cacheMemory.Set("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));
            await _cacheDistributed.SetStringAsync("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));




            var user = await _repository.GetUser(confirmRequest.UserId);

            List<string> userTags = new List<string>();
            if (user != null)
            {
                var accountTokens = await _repository.GetTokens(user.IdentityId);
                if (accountTokens != null)
                {
                    foreach (var token in accountTokens)
                    {
                        userTags.Add(token.NotificationTag);
                    }

                }
            }

            var resDate = DateTime.ParseExact(confirmRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(confirmRequest.EndDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            int duration = (int)(endDate - resDate).TotalMinutes;

            ReservationRequestState requestState = null;
            if (confirmRequest.Acceptance)
            {
                var res = await this.SetConfirmedTableStateCacheData(resDate, duration, confirmRequest.TableId);
                if (!res)
                {
                    return new ServerResponse(StatusCode.NotAvailable);
                }
                var table = await _repository.GetTable(confirmRequest.TableId);
                if (table is null)
                {
                    return new ServerResponse(StatusCode.NotFound);
                }
                var client = table.Hall.Floor.Client;
                var reservation = new Reservation()
                {
                    ChildFree = confirmRequest.IsChildFree,
                    Comments = confirmRequest.Comments,
                    Duration = duration,
                    GuestCount = confirmRequest.GuestCount,
                    ReservationDate = resDate,
                    ReservationStateId = null,
                    AddedByIdentityId = user.IdentityId,
                    IdentityUserId = user.IdentityId,
                    ReservationRequestId = confirmRequest.ReservationRequestId,
                    TableId = confirmRequest.TableId,
                    PetsFree = confirmRequest.IsPetsFree,
                    Invalids = confirmRequest.Invalids,
                    ClientId = client.Id
                };
                try
                {
                    reservation = await _repository.AddReservation(reservation);
                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }
                try
                {
                    foreach (var inviteeId in confirmRequest.InviteeIds)
                    {
                        await _repository.AddInvitee(new Invitee()
                        {
                            ReservationId = reservation.Id,
                            UserId = inviteeId
                        });
                    }

                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }

                try
                {
                    requestState = await _repository.GetReservationRequestState("accepted");
                }
                catch { }


                // notify user
                try
                {
                    _notificationService.SendNotification("reservation accepted", MobilePlatform.gcm, "string", userTags.ToArray());
                }
                catch { }
                //return new ServerResponse(StatusCode.Ok);
            }
            else
            {
                await this.RemoveTableStateCacheData(resDate, duration, confirmRequest.TableId, false);
                // notify user
                try
                {
                    _notificationService.SendNotification("reservation rejected", MobilePlatform.gcm, "string", userTags.ToArray());
                }
                catch { }

                try
                {
                    requestState = await _repository.GetReservationRequestState("rejected");
                }
                catch { }

            }
            var resRequest = await _repository.GetReservationRequest(confirmRequest.ReservationRequestId);
            if (resRequest != null && requestState != null)
            {
                resRequest.ReviewedByIndentityId = clientIdentityId;
                resRequest.ReservationRequestStateId = requestState.Id;
                try
                {
                    resRequest = await _repository.UpdateReservationRequest(resRequest);

                }
                catch { }
            }
            return new ServerResponse(StatusCode.Ok);

        }



        public async Task<ServerResponse> AddConfirmedReservationByOwner(ConfirmReservationRequest confirmRequest, string ownerIdentityId)
        {
            Owner owner;
            try
            {
                owner = await _repository.GetOwner(ownerIdentityId);
                if (owner is null)
                {
                    return new ServerResponse(StatusCode.UserNotFound);
                }
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            var table = await _repository.GetTable(confirmRequest.TableId);
            if (table is null)
            {
                return new ServerResponse(StatusCode.NotFound);
            }
            var client = table.Hall.Floor.Client;
            if (!owner.Organization.Clients.Contains(client))
            {
                return new ServerResponse(StatusCode.NotFound);
            }

            //string json = null;
            //_cacheMemory.TryGetValue("reservationRequests", out json);
            var json = await _cacheDistributed.GetStringAsync("reservationRequests");
            if (json is null)
            {
                return new ServerResponse(StatusCode.Error);
            }
            ICollection<int> reservationRequestsCache = JsonConvert.DeserializeObject<ICollection<int>>(json);
            if (!reservationRequestsCache.Contains(confirmRequest.ReservationRequestId))
            {
                return new ServerResponse(StatusCode.Expired);
            }

            reservationRequestsCache.Remove(confirmRequest.ReservationRequestId);

            //_cacheMemory.Set("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));
            await _cacheDistributed.SetStringAsync("reservationRequests", JsonConvert.SerializeObject(reservationRequestsCache));




            var user = await _repository.GetUser(confirmRequest.UserId);

            List<string> userTags = new List<string>();
            if (user != null)
            {
                var accountTokens = await _repository.GetTokens(user.IdentityId);
                if (accountTokens != null)
                {
                    foreach (var token in accountTokens)
                    {
                        userTags.Add(token.NotificationTag);
                    }

                }
            }

            var resDate = DateTime.ParseExact(confirmRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var endDate = DateTime.ParseExact(confirmRequest.EndDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            int duration = (int)(endDate - resDate).TotalMinutes;
            ReservationRequestState requestState = null;
            if (confirmRequest.Acceptance)
            {
                var res = await this.SetConfirmedTableStateCacheData(resDate, duration, confirmRequest.TableId);
                if (!res)
                {
                    return new ServerResponse(StatusCode.NotAvailable);
                }



                var reservation = new Reservation()
                {
                    ChildFree = confirmRequest.IsChildFree,
                    Comments = confirmRequest.Comments,
                    Duration = duration,
                    GuestCount = confirmRequest.GuestCount,
                    ReservationDate = resDate,
                    ReservationStateId = null,
                    AddedByIdentityId = user.IdentityId,
                    IdentityUserId = user.IdentityId,
                    ReservationRequestId = confirmRequest.ReservationRequestId,
                    TableId = confirmRequest.TableId,
                    PetsFree = confirmRequest.IsPetsFree,
                    Invalids = confirmRequest.Invalids,
                    ClientId = client.Id
                };
                try
                {
                    reservation = await _repository.AddReservation(reservation);
                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }
                try
                {
                    foreach (var inviteeId in confirmRequest.InviteeIds)
                    {
                        await _repository.AddInvitee(new Invitee()
                        {
                            ReservationId = reservation.Id,
                            UserId = inviteeId
                        });
                    }

                }
                catch
                {
                    return new ServerResponse(StatusCode.DbConnectionError);
                }

                try
                {
                    requestState = await _repository.GetReservationRequestState("accepted");
                }
                catch { }


                // notify user
                try
                {
                    _notificationService.SendNotification("reservation accepted", MobilePlatform.gcm, "string", userTags.ToArray());
                }
                catch { }
                //return new ServerResponse(StatusCode.Ok);
            }
            else
            {
                await this.RemoveTableStateCacheData(resDate, duration, confirmRequest.TableId, false);
                // notify user
                try
                {
                    _notificationService.SendNotification("reservation rejected", MobilePlatform.gcm, "string", userTags.ToArray());
                }
                catch { }

                try
                {
                    requestState = await _repository.GetReservationRequestState("rejected");
                }
                catch { }

            }
            var resRequest = await _repository.GetReservationRequest(confirmRequest.ReservationRequestId);
            if (resRequest != null && requestState != null)
            {
                resRequest.ReviewedByIndentityId = ownerIdentityId;
                resRequest.ReservationRequestStateId = requestState.Id;
                try
                {
                    resRequest = await _repository.UpdateReservationRequest(resRequest);

                }
                catch { }
            }
            return new ServerResponse(StatusCode.Ok);

        }










        private UserFullInfoForClient UserToUserFullInfoForClient(User user)
        {
            if (user is null)
            {
                return null;
            }
            return new UserFullInfoForClient()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                BirthDate = user.BirthDate,
                Gender = user.Gender,
                ImagePath = user.ImagePath,
                PhoneNumber = user.Identity.PhoneNumber,
                Email = user.Identity.Email
            };
        }


        //=======================================================================================================


        // CHANGE!!!
        public async Task<ICollection<Reservation>> GetReservations(string identityUserId)
        {
            //var user = await _repository.GetUser(identityUserId);
            //if (user is null)
            //{
            //    return null;
            //}
            //return user.Reservations;

            return new List<Reservation>(); // <- delete this
        }

        public async Task<Reservation> GetReservation(int id)
        {
            return await _repository.GetReservation(id);
        }







        // CHNAGE

        // create Request
        public async Task<ServerResponse> AddReservationByPhone(NewReservationByPhoneRequest reservationRequest, string waiterIdentityId)
        {
            //var resDate = DateTime.ParseExact(reservationRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            //if (resDate < DateTime.Now)
            //{
            //    return new ServerResponse(StatusCode.Expired);
            //}

            //var res = await this.AddTableStateCacheData(resDate, reservationRequest.Duration, reservationRequest.TableIds, true);
            //if (!res)
            //{
            //    return new ServerResponse(StatusCode.NotAvailable);
            //}
            //var table = await _repository.GetTable(reservationRequest.TableIds.FirstOrDefault());
            //if (table is null)
            //{
            //    return new ServerResponse(StatusCode.NotFound);
            //}
            //var client = table.Hall.Floor.Client;
            //var reservation = new Reservation()
            //{
            //    ChildFree = reservationRequest.IsChildFree,
            //    Duration = reservationRequest.Duration,
            //    GuestCount = reservationRequest.GuestCount,
            //    ReservationDate = resDate,
            //    ReservationStateId = null,
            //    AdditionalInfo = reservationRequest.PhoneNumber + " " + reservationRequest.UserName,
            //    Comments = reservationRequest.Comments,
            //    PetsFree = reservationRequest.IsPetsFree,
            //    Invalids = reservationRequest.Invalids,
            //    ClientId = client.Id
            //};
            //try
            //{
            //    reservation = await _repository.AddReservation(reservation);
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //try
            //{
            //    foreach (var tableId in reservationRequest.TableIds)
            //    {
            //        await _repository.AddTableReservation(new TableReservation()
            //        {
            //            ReservationId = reservation.Id,
            //            TableId = tableId
            //        });
            //    }
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}

            //return new ServerResponse(StatusCode.Ok);

            return new ServerResponse(StatusCode.Ok); // <- delete this
        }


        // CHANGE
        public async Task<ServerResponse> CancelReservation(int reservationId, int reasonId, string cancelledByIdentityUserId)
        {
            //var resState = await _repository.GetReservationState("cancelled");
            //var cancelReason = await _repository.GetCancelReason(reasonId);
            //var reservation = await _repository.GetReservation(reservationId);
            //if (resState is null || cancelReason is null || reservation is null)
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //if (reservation.ReservationDate < DateTime.Now)
            //{
            //    return new ServerResponse(StatusCode.Expired);
            //}
            //if (reservation.ReservationState != null)
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //reservation.ReservationStateId = resState.Id;
            //reservation.CancelReasonId = reasonId;
            //reservation.CancelledByIdentityId = cancelledByIdentityUserId;
            //if (reservation.BarTableId is null)
            //{
            //    ICollection<int> tableIds = new List<int>();
            //    foreach (var table in reservation.TableReservations)
            //    {
            //        tableIds.Add(table.TableId);
            //    }

            //    // remove redis data
            //    await this.RemoveTableStateCacheData(reservation.ReservationDate, reservation.Duration, tableIds, true);
            //}
            //else
            //{
            //    await this.RemoveBarStateCacheData(reservation.ReservationDate, reservation.Duration, reservation.BarTableId ?? default, true, reservation.GuestCount);
            //}
            //try
            //{
            //    await _repository.UpdateReservation(reservation);
            //    List<string> notificationTags = new List<string>();
            //    if (cancelReason.IdentityRole.NormalizedName.Equals("USER"))
            //    {
            //        var waiters = reservation.Client.Waiters;
            //        foreach (var item in waiters)
            //        {
            //            // найти старшего официанта
            //        }
            //    }
            //    else if (cancelReason.IdentityRole.NormalizedName.Equals("CLIENT"))
            //    {
            //        var user = reservation.User;
            //        if (user != null)
            //        {
            //            var tokens = await _repository.GetTokens(user.IdentityId);
            //            foreach (var item in tokens)
            //            {
            //                notificationTags.Add(item.NotificationTag);
            //            }
            //        }
            //    }
            //    else if (cancelReason.IdentityRole.NormalizedName.Equals("ADMIN"))
            //    {

            //    }

            //    _notificationService.SendNotification("Cancel reservation", MobilePlatform.gcm, "string", notificationTags.ToArray());



            //    return new ServerResponse(StatusCode.Ok);
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}

            return new ServerResponse(StatusCode.Ok);          // <- delete this

        }


        // CHANGE !!!
        public async Task<ServerResponse> CompleteReservation(int reservationId)
        {
            //var resState = await _repository.GetReservationState("completed");
            //var reservation = await _repository.GetReservation(reservationId);
            //if (resState is null || reservation is null)
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //reservation.ReservationStateId = resState.Id;
            //if (reservation.BarId is null)
            //{
            //    ICollection<int> tableIds = new List<int>();
            //    foreach (var table in reservation.TableReservations)
            //    {
            //        tableIds.Add(table.TableId);
            //    }

            //    // remove redis data
            //    await this.RemoveTableStateCacheData(reservation.ReservationDate, reservation.Duration, tableIds, true);
            //}
            //else
            //{
            //    await this.RemoveBarStateCacheData(reservation.ReservationDate, reservation.Duration, reservation.BarTableId ?? default, true, reservation.GuestCount);
            //}
            //try
            //{
            //    await _repository.UpdateReservation(reservation);
            //    return new ServerResponse(StatusCode.Ok);
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}

            return new ServerResponse(StatusCode.Ok); // <- delete this
        }


        // CHANGE !!!
        public async Task<ServerResponse> ChangeTable(ChangeReservationTablesRequest changeRequest)
        {
            //var reservation = await _repository.GetReservation(changeRequest.ReservationId);
            //if (reservation is null)
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //ICollection<int> tableIdsPrev = new List<int>();
            //foreach (var table in reservation.TableReservations)
            //{
            //    tableIdsPrev.Add(table.TableId);
            //}

            //// change redis data
            //var res = await this.ChangeTableStateCacheData(reservation.ReservationDate, reservation.ReservationDate, reservation.Duration, reservation.Duration, tableIdsPrev, changeRequest.TableIds, true);
            //if (!res)
            //{
            //    return new ServerResponse(StatusCode.NotAvailable);
            //}
            //await _repository.DeleteTableReservations(changeRequest.ReservationId);

            //foreach (var tableId in changeRequest.TableIds)
            //{
            //    await _repository.AddTableReservation(new TableReservation()
            //    {
            //        ReservationId = changeRequest.ReservationId,
            //        TableId = tableId
            //    });
            //}
            //return new ServerResponse(StatusCode.Ok);

            return new ServerResponse(StatusCode.Ok); // <- delete this


        }


        // CHANGE
        public async Task<ICollection<TableCurrentStateCacheData>> GetTablesStates(TableState getStateRequest)
        {
            //var interval = 15;  // add to appsettings
            //ICollection<TableCurrentStateCacheData> tableStates = null;
            //var json = await _cacheDistributed.GetStringAsync("tableStates");
            //if (json is null)
            //{
            //    return null;
            //}

            //tableStates = JsonConvert.DeserializeObject<ICollection<TableCurrentStateCacheData>>(json);
            //if (tableStates is null)
            //{
            //    return null;
            //}

            //var res = new List<TableCurrentStateCacheData>();
            //var resDate = DateTime.ParseExact(getStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //for (int i = 0; i < getStateRequest.Duration; i += interval)
            //{
            //    foreach (var id in getStateRequest.TableIds)
            //    {

            //        var tStateCache = tableStates.FirstOrDefault(s => s.DateTime.Equals(resDate.AddMinutes(interval)) && s.TableId == id);
            //        if (tStateCache != null)
            //        {
            //            res.Add(tStateCache);
            //        }
            //    }
            //}

            //return res;

            return new List<TableCurrentStateCacheData>(); // <- delete this
        }





        private async Task<bool> AddTableStateCacheData(DateTime timeStart, int duration, int tableId, bool isConfirmed)
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

            json = JsonConvert.SerializeObject(tableStates);
            await _cacheDistributed.SetStringAsync("tableStates", json);
            return true;
        }

        private async Task<bool> SetConfirmedTableStateCacheData(DateTime timeStart, int duration, int tableId)
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

            json = JsonConvert.SerializeObject(tableStates);
            await _cacheDistributed.SetStringAsync("tableStates", json);
            return true;
        }

        private async Task RemoveTableStateCacheData(DateTime timeStart, int duration, int tableId, bool isConfirmed)
        {
            var interval = 15;  // add to appsettings

            var json = await _cacheDistributed.GetStringAsync("tableStates");

            if (json is null)
            {
                return;
            }
            ICollection<TableCurrentStateCacheData> tableStates = JsonConvert.DeserializeObject<ICollection<TableCurrentStateCacheData>>(json);

            for (int i = 0; i < duration; i += interval)
            {
                var tableState = tableStates.FirstOrDefault(t => t.TableId == tableId && t.DateTime == timeStart.AddMinutes(i));

                if (tableState != null)
                {
                    tableStates.Remove(tableState);
                }
            }

            json = JsonConvert.SerializeObject(tableStates);

            await _cacheDistributed.SetStringAsync("tableStates", json);
        }

        private async Task<bool> ChangeTableStateCacheData(DateTime timePrev, DateTime timeNew, int durationPrev, int durationNew, int tableIdPrev, int tableIdNew, bool isConfirmed)
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
            for (int i = 0; i < durationPrev; i += interval)
            {
                var tableState = tableStates.FirstOrDefault(t => t.TableId == tableIdPrev && t.DateTime == timePrev.AddMinutes(i));
                if (tableState != null)
                {
                    tableStates.Remove(tableState);
                }
            }

            // add new states
            for (int i = 0; i < durationNew; i += interval)
            {
                var tableState = tableStates.FirstOrDefault(t => t.TableId == tableIdNew && t.DateTime == timePrev.AddMinutes(i));
                if (tableState != null)
                {
                    return false;
                }
                tableStates.Add(new TableCurrentStateCacheData()
                {
                    TableId = tableIdNew,
                    IsConfirmed = isConfirmed,
                    DateTime = timeNew.AddMinutes(i)
                });
            }

            json = JsonConvert.SerializeObject(tableStates);

            await _cacheDistributed.SetStringAsync("tableStates", json);
            return true;
        }





        // BAR TABLES


        private async Task HandleBarPendingTimer(string key, Timer timer)
        {
            //string json = null;
            //_cacheMemory.TryGetValue(key, out json);
            var json = await _cacheDistributed.GetStringAsync(key);
            if (json != null)
            {
                //_cacheMemory.Remove(key);
                await _cacheDistributed.RemoveAsync(key);

                BarStates barStateRequest = JsonConvert.DeserializeObject<BarStates>(json);
                if (barStateRequest != null)
                {
                    var tableBar = await _repository.GetBar(barStateRequest.BarId);
                    if (tableBar != null)
                    {
                        await this.RemoveBarStateCacheData(DateTime.ParseExact(barStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), barStateRequest.Duration, barStateRequest.BarId, false, barStateRequest.GuestCount);
                    }
                }

            }
            timer.Stop();


        }

        public async Task<ServerResponse<string>> SetBarPendingTableState(BarStates stateRequest)
        {
            var resDate = DateTime.ParseExact(stateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            var barTable = await _repository.GetBar(stateRequest.BarId);
            if (barTable is null)
            {
                return new ServerResponse<string>(StatusCode.Error, null);
            }
            try
            {
                var res = await this.AddBarStateCacheData(resDate, stateRequest.Duration, stateRequest.BarId, stateRequest.GuestCount, false, barTable.MaxGuestCount);
                if (res)
                {
                    var key = Guid.NewGuid().ToString();
                    int timerTime = 2 * 60 * 1000; // add to appsettings
                    Timer timer = new Timer(timerTime);
                    timer.Elapsed += async (sender, e) => await HandleBarPendingTimer(key, timer);

                    //_cacheMemory.Set(key, JsonConvert.SerializeObject(stateRequest));
                    await _cacheDistributed.SetStringAsync(key, JsonConvert.SerializeObject(stateRequest));

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


        // CHANGE !!!
        public async Task<ServerResponse<Reservation>> AddNewBarReservation(NewBarReservationRequest newReservationRequest, string identityId)
        {
            // //string json = null;
            // //_cacheMemory.TryGetValue(newReservationRequest.Code, out json);
            //var json = await _cacheDistributed.GetStringAsync(newReservationRequest.Code);

            //if (json != null)
            //{
            //    _cacheMemory.Remove(newReservationRequest.Code);
            //    await _cacheDistributed.RemoveAsync(newReservationRequest.Code);

            //    var barState = JsonConvert.DeserializeObject<BarStatesRequest>(json);
            //    if (barState != null)
            //    {
            //        var user = await _repository.GetUser(identityId);
            //        var barTable = await _repository.GetBarTable(barState.BarId);
            //        if (barState is null)
            //        {
            //            return new ServerResponse<Reservation>(StatusCode.Error, null);
            //        }

            //        var client = barTable.Hall.Floor.Client;
            //        var resDate = DateTime.ParseExact(barState.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);


            //        if (client.BarReserveDurationAvg >= barState.Duration)
            //        {
            //            var res = await this.SetConfirmedBarStateCacheData(resDate, barState.Duration, barState.BarId, barState.GuestCount, barTable.MaxGuestCount);

            //            if (!res)
            //            {
            //                return new ServerResponse<Reservation>(StatusCode.NotAvailable, null);
            //            }
            //            var reservation = new Reservation()
            //            {
            //                UserId = user.Id,
            //                ChildFree = true,
            //                GuestCount = barState.GuestCount,
            //                Comments = newReservationRequest.Comments,
            //                ReservationDate = resDate,
            //                Duration = barState.Duration,
            //                ReservationStateId = null,
            //                BarTableId = barState.BarId,
            //                PetsFree = true,
            //                Invalids = false,
            //                ClientId = client.Id
            //            };
            //            try
            //            {
            //                reservation = await _repository.AddReservation(reservation);
            //            }
            //            catch
            //            {
            //                return new ServerResponse<Reservation>(StatusCode.Error, null);
            //            }


            //            if (client != null)
            //            {
            //                var waiters = client.Waiters;
            //                if (waiters != null)
            //                {
            //                    List<string> tags = new List<string>();
            //                    foreach (var waiter in waiters)
            //                    {
            //                        var tokens = await _repository.GetTokens(waiter.IdentityId);
            //                        foreach (var t in tokens)
            //                        {
            //                            tags.Add(t.NotificationTag);
            //                        }
            //                    }
            //                    try
            //                    {
            //                        _notificationService.SendNotification("New reservation", MobilePlatform.gcm, "string", tags.ToArray());
            //                    }
            //                    catch
            //                    {
            //                        return new ServerResponse<Reservation>(StatusCode.SendingNotificationError, reservation);
            //                    }
            //                }
            //            }
            //            return new ServerResponse<Reservation>(reservation);
            //        }
            //        else
            //        {


            //            var result = await SendBarReservationOnConfirmation(barState, newReservationRequest, user.Id, client);
            //            return new ServerResponse<Reservation>(result.StatusCode, null);

            //        }
            //    }
            //    else
            //    {
            //        return new ServerResponse<Reservation>(StatusCode.Error, null);
            //    }
            //}
            //else
            //{
            //    return new ServerResponse<Reservation>(StatusCode.Expired, null);
            //}

            return new ServerResponse<Reservation>(StatusCode.Ok, null); // <- delete this
        }



        private async Task HandleBarConfirmationTimer(string key, Timer timer, IEnumerable<Waiter> waiters)
        {
            timer.Stop();
            //string json = null;
            //_cacheMemory.TryGetValue(key, out json);
            var json = await _cacheDistributed.GetStringAsync(key);
            if (json != null)
            {
                //_cacheMemory.Remove(key);
                await _cacheDistributed.RemoveAsync(key);

                ConfirmBarReservationRequest confirmRequest = JsonConvert.DeserializeObject<ConfirmBarReservationRequest>(json);
                if (confirmRequest != null)
                {
                    await this.RemoveBarStateCacheData(DateTime.ParseExact(confirmRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture), confirmRequest.Duration, confirmRequest.BarId, false, confirmRequest.GuestCount);
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
                var user = await _repository.GetUser(confirmRequest.UserId);
                if (user != null)
                {
                    List<string> userTags = new List<string>();
                    var accountTokens = await _repository.GetTokens(user.IdentityId);
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

            }


        }


        public async Task<ServerResponse> SendBarReservationOnConfirmation(BarStates barStateRequest, NewBarReservationRequest reservationRequest, int userId, Client client)
        {
            var resDate = DateTime.ParseExact(barStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            var key = Guid.NewGuid().ToString();
            var closeToday = DateTime.Today.AddMinutes(client.CloseTime);
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
            timer.Elapsed += async (sender, e) => await HandleBarConfirmationTimer(key, timer, client.Waiters);
            ConfirmBarReservationRequest confirmRequest = new ConfirmBarReservationRequest()
            {
                Acceptance = false,
                Code = key,
                Duration = barStateRequest.Duration,
                StartDateTime = barStateRequest.StartDateTime,
                BarId = barStateRequest.BarId,
                UserId = userId,
                Comments = reservationRequest.Comments,
                GuestCount = barStateRequest.GuestCount
            };

            //_cacheMemory.Set(key, JsonConvert.SerializeObject(confirmRequest));
            await _cacheDistributed.SetStringAsync(key, JsonConvert.SerializeObject(confirmRequest));
            timer.Start();

            // уведомлять старшего(-их) официанта(-ов)

            if (client.Waiters != null)
            {
                List<string> tags = new List<string>();
                foreach (var waiter in client.Waiters)
                {
                    var tokens = await _repository.GetTokens(waiter.IdentityId);
                    foreach (var t in tokens)
                    {
                        tags.Add(t.NotificationTag);
                    }
                }
                try
                {
                    _notificationService.SendNotification("New bar reservation for confirmation", MobilePlatform.gcm, "string", tags.ToArray());
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


        // CHANGE !!!
        public async Task<ServerResponse> AddBarConfirmedReservation(ConfirmBarReservationRequest confirmRequest)
        {
            // //string json = null;
            // //_cacheMemory.TryGetValue(confirmRequest.Code, out json);
            //var json = await _cacheDistributed.GetStringAsync(confirmRequest.Code);
            //if (json is null)
            //{
            //    return new ServerResponse(StatusCode.Expired);
            //}
            // //_cacheMemory.Remove(confirmRequest.Code);
            //await _cacheDistributed.RemoveAsync(confirmRequest.Code);
            //NewBarReservationRequest reservationRequest = JsonConvert.DeserializeObject<NewBarReservationRequest>(json);
            //if (reservationRequest is null)
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //var user = await _repository.GetUser(confirmRequest.UserId);
            //List<string> userTags = new List<string>();
            //if (user != null)
            //{
            //    var accountTokens = await _repository.GetTokens(user.IdentityId);
            //    if (accountTokens != null)
            //    {
            //        foreach (var token in accountTokens)
            //        {
            //            userTags.Add(token.NotificationTag);
            //        }

            //    }
            //}

            //var resDate = DateTime.ParseExact(confirmRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            //if (confirmRequest.Acceptance)
            //{
            //    var barTable = await _repository.GetBarTable(confirmRequest.BarId);
            //    if (barTable is null)
            //    {
            //        return new ServerResponse(StatusCode.Error);
            //    }

            //    var res = await this.SetConfirmedBarStateCacheData(resDate, confirmRequest.Duration, confirmRequest.BarId, confirmRequest.GuestCount, barTable.MaxGuestCount);

            //    if (!res)
            //    {
            //        return new ServerResponse(StatusCode.NotAvailable);
            //    }

            //    var client = barTable.Hall.Floor.Client;

            //    var reservation = new Reservation()
            //    {
            //        ChildFree = true,
            //        Comments = confirmRequest.Comments,
            //        Duration = confirmRequest.Duration,
            //        GuestCount = confirmRequest.GuestCount,
            //        ReservationDate = resDate,
            //        ReservationStateId = null,
            //        UserId = confirmRequest.UserId,
            //        BarTableId = confirmRequest.BarId,
            //        PetsFree = true,
            //        Invalids = false,
            //        ClientId = client.Id
            //    };
            //    try
            //    {
            //        reservation = await _repository.AddReservation(reservation);
            //    }
            //    catch
            //    {
            //        return new ServerResponse(StatusCode.Error);
            //    }


            //    // notify user
            //    try
            //    {
            //        _notificationService.SendNotification("reservation accepted", MobilePlatform.gcm, "string", userTags.ToArray());
            //    }
            //    catch { }
            //    return new ServerResponse(StatusCode.Ok);
            //}
            //else
            //{
            //    await this.RemoveBarStateCacheData(resDate, confirmRequest.Duration, confirmRequest.BarId, false, confirmRequest.GuestCount);
            //    // notify user
            //    try
            //    {
            //        _notificationService.SendNotification("reservation rejected", MobilePlatform.gcm, "string", userTags.ToArray());
            //    }
            //    catch { }
            //}

            //return new ServerResponse(StatusCode.Ok);

            return new ServerResponse(StatusCode.Ok); // <- delete this
        }


        // CHANGE !!!
        public async Task<ServerResponse> AddBarReservationByPhone(NewBarReservationByPhoneRequest reservationRequest)
        {
            //var resDate = DateTime.ParseExact(reservationRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            //if (resDate < DateTime.Now)
            //{
            //    return new ServerResponse(StatusCode.Expired);
            //}
            //var barTable = await _repository.GetBarTable(reservationRequest.BarId);
            //if (barTable is null)
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}
            //var client = barTable.Hall.Floor.Client;

            //var res = await this.AddBarStateCacheData(resDate, reservationRequest.Duration, reservationRequest.BarId, reservationRequest.GuestCount, true, barTable.MaxGuestCount);
            //if (!res)
            //{
            //    return new ServerResponse(StatusCode.NotAvailable);
            //}
            //var reservation = new Reservation()
            //{
            //    BarTableId = reservationRequest.BarId,
            //    ChildFree = true,
            //    PetsFree = true,
            //    Invalids = false,
            //    Duration = reservationRequest.Duration,
            //    GuestCount = reservationRequest.GuestCount,
            //    ReservationDate = resDate,
            //    ReservationStateId = null,
            //    AdditionalInfo = reservationRequest.PhoneNumber + " " + reservationRequest.UserName,
            //    Comments = reservationRequest.Comments,
            //    ClientId = client.Id
            //};
            //try
            //{
            //    reservation = await _repository.AddReservation(reservation);
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}

            //return new ServerResponse(StatusCode.Ok);

            return new ServerResponse(StatusCode.Ok); // <- delete this
        }

        public async Task<ICollection<BarCurrentStateCacheData>> GetBarStates(BarStates getStateRequest)
        {
            var interval = 15;  // add to appsettings
            ICollection<BarCurrentStateCacheData> barStates = null;
            var json = await _cacheDistributed.GetStringAsync("barStates");
            if (json is null)
            {
                return null;
            }

            barStates = JsonConvert.DeserializeObject<ICollection<BarCurrentStateCacheData>>(json);
            if (barStates is null)
            {
                return null;
            }

            var res = new List<BarCurrentStateCacheData>();
            var resDate = DateTime.ParseExact(getStateRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);
            for (int i = 0; i < getStateRequest.Duration; i += interval)
            {
                var bStateCache = barStates.FirstOrDefault(s => s.DateTime.Equals(resDate.AddMinutes(interval)) && s.BarId == getStateRequest.BarId);
                if (bStateCache != null)
                {
                    res.Add(bStateCache);
                }
            }

            return res;

        }





        private async Task<bool> AddBarStateCacheData(DateTime timeStart, int duration, int barId, int guestCount, bool isConfirmed, int maxBarGuestDefault)
        {
            var interval = 15;  // add to appsettings
            var json = await _cacheDistributed.GetStringAsync("barStates");
            ICollection<BarCurrentStateCacheData> barStates;
            if (json != null)
            {
                barStates = JsonConvert.DeserializeObject<ICollection<BarCurrentStateCacheData>>(json);
            }
            else
            {
                barStates = new List<BarCurrentStateCacheData>();
            }

            for (int i = 0; i < duration; i += interval)
            {
                var barState = barStates.FirstOrDefault(b => b.BarId == barId && b.DateTime == timeStart.AddMinutes(i));
                if (barState is null)
                {
                    barStates.Add(new BarCurrentStateCacheData()
                    {
                        BarId = barId,
                        DateTime = timeStart.AddMinutes(i),
                        ConfirmedCount = isConfirmed ? guestCount : 0,
                        NotConfirmedCount = isConfirmed ? 0 : guestCount
                    });
                }
                else if (maxBarGuestDefault - barState.ConfirmedCount - barState.NotConfirmedCount <= guestCount)
                {
                    barState.NotConfirmedCount += guestCount;
                }
                else
                {
                    return false;
                }
            }

            json = JsonConvert.SerializeObject(barStates);
            await _cacheDistributed.SetStringAsync("barStates", json);
            return true;
        }


        private async Task<bool> SetConfirmedBarStateCacheData(DateTime timeStart, int duration, int barId, int guestCount, int maxBarGuestDefault)
        {
            var interval = 15;  // add to appsettings

            var json = await _cacheDistributed.GetStringAsync("barStates");
            ICollection<BarCurrentStateCacheData> barStates;
            if (json != null)
            {
                barStates = JsonConvert.DeserializeObject<ICollection<BarCurrentStateCacheData>>(json);
            }
            else
            {
                barStates = new List<BarCurrentStateCacheData>();

            }

            for (int i = 0; i < duration; i += interval)
            {
                var barState = barStates.FirstOrDefault(b => b.BarId == barId && b.DateTime == timeStart.AddMinutes(i));
                if (barState is null)
                {
                    barStates.Add(new BarCurrentStateCacheData()
                    {
                        BarId = barId,
                        DateTime = timeStart.AddMinutes(i),
                        ConfirmedCount = guestCount,
                        NotConfirmedCount = 0
                    });
                }
                else if (barState.NotConfirmedCount >= guestCount)
                {

                    barState.NotConfirmedCount -= guestCount;
                    barState.ConfirmedCount += guestCount;
                }
                else if (maxBarGuestDefault - barState.ConfirmedCount - barState.NotConfirmedCount < guestCount)
                {
                    barState.NotConfirmedCount += guestCount;
                }
                else
                {
                    return false;
                }
            }

            json = JsonConvert.SerializeObject(barStates);
            await _cacheDistributed.SetStringAsync("barStates", json);
            return true;
        }


        private async Task RemoveBarStateCacheData(DateTime timeStart, int duration, int barId, bool isConfirmed, int guestCount)
        {
            var interval = 15;  // add to appsettings

            var json = await _cacheDistributed.GetStringAsync("barStates");

            if (json is null)
            {
                return;
            }
            ICollection<BarCurrentStateCacheData> barStates = JsonConvert.DeserializeObject<ICollection<BarCurrentStateCacheData>>(json);

            for (int i = 0; i < duration; i += interval)
            {
                var barState = barStates.FirstOrDefault(b => b.BarId == barId && b.DateTime == timeStart.AddMinutes(i));

                if (barState != null)
                {
                    if (isConfirmed)
                    {
                        if (barState.ConfirmedCount >= guestCount)
                        {
                            barState.ConfirmedCount -= guestCount;
                        }
                        else
                        {
                            barState.ConfirmedCount = 0;
                        }
                    }
                    else
                    {
                        if (barState.NotConfirmedCount >= guestCount)
                        {
                            barState.NotConfirmedCount -= guestCount;
                        }
                        else
                        {
                            barState.NotConfirmedCount = 0;
                        }
                    }
                    if (barState.ConfirmedCount == 0 && barState.NotConfirmedCount == 0)
                    {
                        barStates.Remove(barState);
                    }
                }

            }

            json = JsonConvert.SerializeObject(barStates);

            await _cacheDistributed.SetStringAsync("barStates", json);
        }



        // Visitors


        public async Task<ServerResponse> AddNewVisitor(NewVisitRequest visitRequest, string addedByIdentityId)
        {
            //if (visitRequest.TableId is null && visitRequest.BarId is null)
            //{
            //    return new ServerResponse(StatusCode.Error);
            //}

            //var resDate = DateTime.ParseExact(visitRequest.StartDateTime, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture);

            //bool res;
            //if (visitRequest.BarId is null)
            //{
            //    res = await this.AddTableStateCacheData(resDate, visitRequest.Duration, new List<int>() { visitRequest.TableId ?? default }, true);
            //}
            //else
            //{
            //    Bar barTable;
            //    try
            //    {
            //        barTable = await _repository.GetBar(visitRequest.BarId ?? default);
            //        if (barTable is null)
            //        {
            //            return new ServerResponse(StatusCode.NotFound);
            //        }
            //    }
            //    catch
            //    {
            //        return new ServerResponse(StatusCode.DbConnectionError);
            //    }

            //    res = await this.AddBarStateCacheData(resDate, visitRequest.Duration, visitRequest.BarId ?? default, visitRequest.GuestCount, true, barTable.MaxGuestCount);
            //}

            //if (!res)
            //{
            //    return new ServerResponse(StatusCode.NotAvailable);
            //}

            //Visitor visitor = new Visitor()
            //{
            //    AddedByIdentityId = addedByIdentityId,
            //    BarId = visitRequest.BarId,
            //    TableId = visitRequest.TableId,
            //    Duration = visitRequest.Duration,
            //    GuestCount = visitRequest.GuestCount,
            //    StartDateTime = resDate,
            //    IsCompleted = false
            //};

            //try
            //{
            //    await _repository.AddVisitor(visitor);
            //    return new ServerResponse(StatusCode.Ok);
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.DbConnectionError);
            //}
            return new ServerResponse(StatusCode.Ok); // <- delete this
        }


        public async Task<ServerResponse> CompleteVisit(int visitId)
        {
            //Visitor visitor;
            //try
            //{
            //    visitor = await _repository.GetVisitor(visitId);
            //    if (visitor is null)
            //    {
            //        return new ServerResponse(StatusCode.NotFound);
            //    }
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.DbConnectionError);
            //}
            //if (visitor.TableId != null)
            //{
            //    await this.RemoveTableStateCacheData(visitor.StartDateTime, visitor.Duration, new List<int>() { visitor.TableId ?? default }, true);
            //}
            //else if (visitor.BarId != null)
            //{
            //    await this.RemoveBarStateCacheData(visitor.StartDateTime, visitor.Duration, visitor.BarId ?? default, true, visitor.GuestCount);
            //}

            //visitor.IsCompleted = true;

            //try
            //{
            //    await _repository.UpdateVisitor(visitor);
            //    return new ServerResponse(StatusCode.Ok);
            //}
            //catch
            //{
            //    return new ServerResponse(StatusCode.DbConnectionError);
            //}

            return new ServerResponse(StatusCode.Ok); // <- delete this
        }

    }



}

//Reservation state:
//-cancelled; - user, waiter
//-completed; - waiter
//-missed; - timer; 
