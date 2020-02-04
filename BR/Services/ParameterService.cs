﻿using BR.DTO;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
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
        public async Task<ICollection<ParameterInfo>> GetAllPaymentTypes()
        {
            var types = await _repository.GetAllPaymentTypes();
            var res = new List<ParameterInfo>();
            foreach (var item in types)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return res;
        }
        public async Task<ICollection<ParameterInfo>> GetAllCuisines()
        {
            var types = await _repository.GetAllCuisines();
            var res = new List<ParameterInfo>();
            foreach (var item in types)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return res;
        }
        public async Task<ICollection<ParameterInfo>> GetAllClientTypes()
        {
            var types = await _repository.GetAllClientTypes();
            var res = new List<ParameterInfo>();
            foreach (var item in types)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return res;
        }

        public async Task<ICollection<ParameterInfo>> GetAllMealType()
        {
            var types = await _repository.GetAllMealTypes();
            var res = new List<ParameterInfo>();
            foreach (var item in types)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return res;
        }

        public async Task AddCuisine(ICollection<string> cuisineTitles)
        {
            foreach (var title in cuisineTitles)
            {
                if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
                {
                    if (await _repository.GetCuisine(title) == null)
                    {
                        try
                        {
                            await _repository.AddCuisine(new Cuisine() { Title = title });
                        }
                        catch { }
                    }
                }
            }
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

        public async Task AddClientType(ICollection<string> clientTypeTitles)
        {
            foreach (var title in clientTypeTitles)
            {
                if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
                {
                    if (await _repository.GetClientType(title) == null)
                    {
                        try
                        {
                            await _repository.AddClientType(new ClientType() { Title = title });
                        }
                        catch { }
                    }
                }
            }

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

        private ParameterInfo ToParameterInfo(int id, string title)
        {
            return new ParameterInfo()
            {
                Id = id,
                Title = title
            };
        }
    }
}
