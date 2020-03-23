using BR.DTO;
using BR.DTO.Parameters;
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

        public async Task UpdateCuisine(ICollection<Cuisine> cuisines)
        {
            if(cuisines is null)
            {
                return;
            }
            foreach (var item in cuisines)
            {
                var cuisineToUpdate = await _repository.GetCuisine(item.Id);
                if (cuisineToUpdate != null)
                {
                    if (await _repository.GetCuisine(item.Title) == null)
                    {
                        cuisineToUpdate.Title = item.Title;
                        try
                        {
                            await _repository.UpdateCuisine(cuisineToUpdate);
                        }
                        catch { }
                    }
                }
            }
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

        public async Task UpdateClientType(ICollection<ClientType> clientTypes)
        {
            if (clientTypes is null)
            {
                return;
            }
            foreach (var item in clientTypes)
            {
                var typeToUpdate = await _repository.GetClientType(item.Id);
                if (typeToUpdate != null)
                {
                    if (!String.IsNullOrEmpty(item.Title) && !String.IsNullOrWhiteSpace(item.Title))
                    {
                        if (await _repository.GetClientType(item.Title) == null)
                        {
                            typeToUpdate.Title = item.Title;
                            try
                            {
                                await _repository.UpdateClientType(typeToUpdate);
                            }
                            catch { }
                        }
                    }
                }
            }
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
