using BR.EF;
using BR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services
{
    public class ParameterService : IParameterService
    {
        private readonly IAsyncRepository _repository;

        public ParameterService(IAsyncRepository repository)
        {
            _repository = repository;
        }
        public async Task<IEnumerable<PaymentType>> GetAllPaymentTypes()
        {
            return await _repository.GetAllPaymentTypes();
        }
        public async Task<IEnumerable<Cuisine>> GetAllCuisines()
        {
            return await _repository.GetAllCuisines();
        }
        public async Task<IEnumerable<ClientType>> GetAllClientTypes()
        {
            return await _repository.GetAllClientTypes();
        }

        public async Task<IEnumerable<MealType>> GetAllMealType()
        {
            return await _repository.GetAllMealTypes();
        }

        public async Task<Cuisine> AddCuisine(string cuisineTitle)
        {
            if (!String.IsNullOrEmpty(cuisineTitle) && !String.IsNullOrWhiteSpace(cuisineTitle))
            {
                if (await _repository.GetCuisine(cuisineTitle) == null)
                {
                    return await _repository.AddCuisine(new Cuisine() { Title = cuisineTitle });
                }
            }
            return null;
        }

        public async Task<Cuisine> UpdateCuisine(Cuisine cuisine)
        {
            var cuisineToUpdate = await _repository.GetCuisine(cuisine.Id);
            if (cuisineToUpdate != null)
            {
                if (await _repository.GetCuisine(cuisine.Title) == null)
                {
                    cuisineToUpdate.Title = cuisine.Title;
                    return await _repository.UpdateCuisine(cuisineToUpdate);
                }
            }
            return null;
        }

        public async Task<bool> DeleteCuisine(int id)
        {
            var cuisine = await _repository.GetCuisine(id);
            if (cuisine != null)
            {
                return await _repository.DeleteCuisine(cuisine);
            }
            return false;
        }

        public async Task<ClientType> AddClientType(string clientTypeTitle)
        {
            if (!String.IsNullOrEmpty(clientTypeTitle) && !String.IsNullOrWhiteSpace(clientTypeTitle))
            {
                if (await _repository.GetClientType(clientTypeTitle) == null)
                {
                    return await _repository.AddClientType(new ClientType() { Title = clientTypeTitle });
                }
            }
            return null;
        }

        public async Task<ClientType> UpdateClientType(ClientType clientType)
        {
            var typeToUpdate = await _repository.GetClientType(clientType.Id);
            if (typeToUpdate != null)
            {
                if (!String.IsNullOrEmpty(clientType.Title) && !String.IsNullOrWhiteSpace(clientType.Title))
                {
                    if (await _repository.GetClientType(clientType.Title) == null)
                    {
                        typeToUpdate.Title = clientType.Title;
                        return await _repository.UpdateClientType(typeToUpdate);
                    }
                }

            }
            return null;
        }

        public async Task<bool> DeleteClientType(int id)
        {
            var clientType = await _repository.GetClientType(id);
            if (clientType != null)
            {
                return await _repository.DeleteClientType(clientType);
            }
            return false;
        }

        public async Task<PaymentType> AddPaymentType(string paymentTypeTitle)
        {
            if (!String.IsNullOrEmpty(paymentTypeTitle) && !String.IsNullOrWhiteSpace(paymentTypeTitle))
            {
                if (await _repository.GetPaymentType(paymentTypeTitle) == null)
                {
                    return await _repository.AddPaymentType(new PaymentType() { Title = paymentTypeTitle });
                }
            }
            return null;
        }

        public async Task<PaymentType> UpdatePaymentType(PaymentType paymentType)
        {
            var paymentTypeToUpdate = await _repository.GetPaymentType(paymentType.Id);
            if (paymentTypeToUpdate != null)
            {
                if (!String.IsNullOrEmpty(paymentType.Title) && !String.IsNullOrWhiteSpace(paymentType.Title))
                {
                    if (await _repository.GetPaymentType(paymentType.Title) == null)
                    {
                        paymentTypeToUpdate.Title = paymentType.Title;
                        return await _repository.UpdatePaymentType(paymentTypeToUpdate);
                    }
                }

            }
            return null;
        }

        public async Task<bool> DeletePaymentType(int id)
        {
            var paymentType = await _repository.GetPaymentType(id);
            if (paymentType != null)
            {
                return await _repository.DeletePaymentType(paymentType);
            }
            return false;
        }
    }
}
