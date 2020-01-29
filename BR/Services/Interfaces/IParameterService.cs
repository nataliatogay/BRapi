using BR.DTO;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IParameterService
    {
        Task<ICollection<ParameterInfo>> GetAllMealType();
        Task<ICollection<ParameterInfo>> GetAllCuisines();
        Task<Cuisine> AddCuisine(string cuisineTitle);
        Task<Cuisine> UpdateCuisine(Cuisine cuisine);
        Task<bool> DeleteCuisine(int id);
        Task<ICollection<ParameterInfo>> GetAllClientTypes();
        Task<ClientType> AddClientType(string clientTypeTitle);
        Task<ClientType> UpdateClientType(ClientType clientType);
        Task<bool> DeleteClientType(int id);
        Task<ICollection<ParameterInfo>> GetAllPaymentTypes();
        Task<PaymentType> AddPaymentType(string paymentTypeTitle);
        Task<PaymentType> UpdatePaymentType(PaymentType paymentType);
        Task<bool> DeletePaymentType(int id);

    }
}
