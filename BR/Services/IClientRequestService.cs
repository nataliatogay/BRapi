﻿using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IClientRequestService
    {
        Task<IEnumerable<ClientRequest>> GetAllClientRequests();
        Task<ClientRequest> GetClientRequest(int id);
        Task AddNewClientRequest(ClientRequest clientRequest);
        Task AddNewClientRequest(NewRequestRequest newClientRequest);
        Task<int> ClientRequestCount();
        

    }
}
