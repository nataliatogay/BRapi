using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public interface IParameterService
    {
        Task<IEnumerable<MealType>> GetAllMealType();
        Task<IEnumerable<Cuisine>> GetAllCuisines();
        Task<Cuisine> AddCuisine(string cuisineTitle);
        Task<Cuisine> UpdateCuisine(Cuisine cuisine);
        Task<bool> DeleteCuisine(int id);
        Task<IEnumerable<ClientType>> GetAllClientTypes();
        Task<ClientType> AddClientType(string clientTypeTitle);
        Task<ClientType> UpdateClientType(ClientType clientType);
        Task<bool> DeleteClientType(int id);
        Task<IEnumerable<PaymentType>> GetAllPaymentTypes();
        Task<PaymentType> AddPaymentType(string paymentTypeTitle);
        Task<PaymentType> UpdatePaymentType(PaymentType paymentType);
        Task<bool> DeletePaymentType(int id);

    }
}
