using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IReservationService
    {
        Task AddNewReservation(NewReservationRequest newReservationRequest, string identityId);
    }
}
