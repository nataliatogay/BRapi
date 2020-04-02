using BR.DTO;
using BR.DTO.Parameters;
using BR.EF;
using BR.Models;
using BR.Services.Interfaces;
using BR.Utils;
using Microsoft.EntityFrameworkCore;
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


        public async Task<ServerResponse<ClientParametersInfoResponse>> GetClientParamenters()
        {
            var cuisines = await this.GetAllCuisines();
            var clientTypes = await this.GetAllClientTypes();
            var mealTypes = await this.GetAllMealTypes();
            var features = await this.GetAllFeatures();
            var dishes = await this.GetAllDishes();
            var specialDiets = await this.GetAllSpecialDiets();
            var goodFors = await this.GetAllGoodFors();

            if (cuisines.StatusCode == StatusCode.Ok &&
                clientTypes.StatusCode == StatusCode.Ok &&
                mealTypes.StatusCode == StatusCode.Ok &&
                features.StatusCode == StatusCode.Ok &&
                dishes.StatusCode == StatusCode.Ok &&
                specialDiets.StatusCode == StatusCode.Ok &&
                goodFors.StatusCode == StatusCode.Ok)
            {
                return new ServerResponse<ClientParametersInfoResponse>(
                    StatusCode.Ok,
                    new ClientParametersInfoResponse()
                    {
                        ClientTypes = clientTypes.Data,
                        Cuisines = cuisines.Data,
                        Dishes = dishes.Data,
                        Features = features.Data,
                        GoodFors = goodFors.Data,
                        MealTypes = mealTypes.Data,
                        SpecialDiets = specialDiets.Data
                    });
            }

            else
            {
                return new ServerResponse<ClientParametersInfoResponse>(StatusCode.DbConnectionError, null);
            }





        }

        public async Task<ServerResponse<ICollection<ParameterInfo>>> GetAllMealTypes()
        {
            ICollection<MealType> mealTypes;
            try
            {
                mealTypes = await _repository.GetAllMealTypes();
            }
            catch
            {
                return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.DbConnectionError, null);
            }
            var res = new List<ParameterInfo>();
            foreach (var item in mealTypes)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ICollection<ParameterInfo>>> GetAllCuisines()
        {
            ICollection<Cuisine> types;
            try
            {
                types = await _repository.GetAllCuisines();
            }
            catch
            {
                return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.DbConnectionError, null);
            }

            var res = new List<ParameterInfo>();
            foreach (var item in types)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ICollection<ParameterInfo>>> GetAllClientTypes()
        {
            ICollection<ClientType> types;
            try
            {
                types = await _repository.GetAllClientTypes();
            }
            catch
            {
                return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.DbConnectionError, null);
            }

            var res = new List<ParameterInfo>();
            foreach (var item in types)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ICollection<ParameterInfo>>> GetAllSpecialDiets()
        {
            ICollection<SpecialDiet> diets;
            try
            {
                diets = await _repository.GetAllSpecialDiets();
            }
            catch
            {
                return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.DbConnectionError, null);
            }

            var res = new List<ParameterInfo>();
            foreach (var item in diets)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ICollection<ParameterInfo>>> GetAllDishes()
        {
            ICollection<Dish> dishes;
            try
            {
                dishes = await _repository.GetAllDishes();
            }
            catch
            {
                return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.DbConnectionError, null);
            }

            var res = new List<ParameterInfo>();
            foreach (var item in dishes)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ICollection<ParameterInfo>>> GetAllFeatures()
        {
            ICollection<Feature> features;
            try
            {
                features = await _repository.GetAllFeatures();
            }
            catch
            {
                return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.DbConnectionError, null);
            }

            var res = new List<ParameterInfo>();
            foreach (var item in features)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.Ok, res);
        }

        public async Task<ServerResponse<ICollection<ParameterInfo>>> GetAllGoodFors()
        {
            ICollection<GoodFor> types;
            try
            {
                types = await _repository.GetAllGoodFors();
            }
            catch
            {
                return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.DbConnectionError, null);
            }

            var res = new List<ParameterInfo>();
            foreach (var item in types)
            {
                res.Add(ToParameterInfo(item.Id, item.Title));
            }
            return new ServerResponse<ICollection<ParameterInfo>>(StatusCode.Ok, res);
        }


        public async Task<ServerResponse<ParameterInfo>> AddCuisine(string title)
        {
            title = title.Trim();
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
            {
                try
                {
                    var res = await _repository.AddCuisine(new Cuisine() { Title = title });
                    return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                }
                catch (DbUpdateException)
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                }
                catch
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> AddClientType(string title)
        {
            title = title.Trim();
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
            {
                try
                {
                    var res = await _repository.AddClientType(new ClientType() { Title = title });
                    return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                }
                catch (DbUpdateException)
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                }
                catch
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> AddGoodFor(string title)
        {
            title = title.Trim();
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
            {
                try
                {
                    var res = await _repository.AddGoodFor(new GoodFor() { Title = title });
                    return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                }
                catch (DbUpdateException)
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                }
                catch
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> AddFeature(string title)
        {
            title = title.Trim();
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
            {
                try
                {
                    var res = await _repository.AddFeature(new Feature() { Title = title, Editable = true });
                    return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                }
                catch (DbUpdateException)
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                }
                catch
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> AddDish(string title)
        {
            title = title.Trim();
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
            {
                try
                {
                    var res = await _repository.AddDish(new Dish() { Title = title });
                    return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                }
                catch (DbUpdateException)
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                }
                catch
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> AddSpecialDiet(string title)
        {
            title = title.Trim();
            if (!String.IsNullOrEmpty(title) && !String.IsNullOrWhiteSpace(title))
            {
                try
                {
                    var res = await _repository.AddSpecialDiet(new SpecialDiet() { Title = title });
                    return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                }
                catch (DbUpdateException)
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                }
                catch
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> UpdateCuisine(Cuisine cuisine)
        {
            var cuisineToUpdate = await _repository.GetCuisine(cuisine.Id);
            if (cuisineToUpdate != null)
            {
                if (!String.IsNullOrEmpty(cuisine.Title) && !String.IsNullOrWhiteSpace(cuisine.Title))
                {
                    cuisineToUpdate.Title = cuisine.Title.Trim();
                    try
                    {
                        var res = await _repository.UpdateCuisine(cuisineToUpdate);
                        return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                    }
                    catch (DbUpdateException)
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                    }
                    catch
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                    }
                }
                else
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.NotFound, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> UpdateClientType(ClientType clientType)
        {
            var typeToUpdate = await _repository.GetClientType(clientType.Id);
            if (typeToUpdate != null)
            {
                if (!String.IsNullOrEmpty(clientType.Title) && !String.IsNullOrWhiteSpace(clientType.Title))
                {
                    typeToUpdate.Title = clientType.Title.Trim();
                    try
                    {
                        var res = await _repository.UpdateClientType(typeToUpdate);
                        return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                    }
                    catch (DbUpdateException)
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                    }
                    catch
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                    }
                }
                else
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.NotFound, null);
            }
        }


        public async Task<ServerResponse<ParameterInfo>> UpdateGoodFor(GoodFor goodFor)
        {
            var goodForToUpdate = await _repository.GetGoodFor(goodFor.Id);
            if (goodForToUpdate != null)
            {
                if (!String.IsNullOrEmpty(goodFor.Title) && !String.IsNullOrWhiteSpace(goodFor.Title))
                {
                    goodForToUpdate.Title = goodFor.Title.Trim();
                    try
                    {
                        var res = await _repository.UpdateGoodFor(goodForToUpdate);
                        return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                    }
                    catch (DbUpdateException)
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                    }
                    catch
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                    }
                }
                else
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.NotFound, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> UpdateSpecialDiet(SpecialDiet diet)
        {
            var dietToUpdate = await _repository.GetSpecialDiet(diet.Id);
            if (dietToUpdate != null)
            {
                if (!String.IsNullOrEmpty(diet.Title) && !String.IsNullOrWhiteSpace(diet.Title))
                {
                    dietToUpdate.Title = diet.Title.Trim();
                    try
                    {
                        var res = await _repository.UpdateSpecialDiet(dietToUpdate);
                        return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                    }
                    catch (DbUpdateException)
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                    }
                    catch
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                    }
                }
                else
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.NotFound, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> UpdateDish(Dish dish)
        {
            var dishForToUpdate = await _repository.GetDish(dish.Id);
            if (dishForToUpdate != null)
            {
                if (!String.IsNullOrEmpty(dish.Title) && !String.IsNullOrWhiteSpace(dish.Title))
                {
                    dishForToUpdate.Title = dish.Title.Trim();
                    try
                    {
                        var res = await _repository.UpdateDish(dishForToUpdate);
                        return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                    }
                    catch (DbUpdateException)
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                    }
                    catch
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                    }
                }
                else
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.NotFound, null);
            }
        }

        public async Task<ServerResponse<ParameterInfo>> UpdateFeature(Feature feature)
        {
            var featureForToUpdate = await _repository.GetFeature(feature.Id);
            if (featureForToUpdate != null && featureForToUpdate.Editable)
            {
                if (!String.IsNullOrEmpty(feature.Title) && !String.IsNullOrWhiteSpace(feature.Title))
                {
                    featureForToUpdate.Title = feature.Title.Trim();
                    try
                    {
                        var res = await _repository.UpdateFeature(featureForToUpdate);
                        return new ServerResponse<ParameterInfo>(StatusCode.Ok, this.ToParameterInfo(res.Id, res.Title));
                    }
                    catch (DbUpdateException)
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Duplicate, null);
                    }
                    catch
                    {
                        return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                    }
                }
                else
                {
                    return new ServerResponse<ParameterInfo>(StatusCode.Error, null);
                }
            }
            else
            {
                return new ServerResponse<ParameterInfo>(StatusCode.NotFound, null);
            }
        }


        public async Task<ServerResponse> DeleteCuisine(int id)
        {
            Cuisine cuisine;
            try
            {
                cuisine = await _repository.GetCuisine(id);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (cuisine != null)
            {
                try
                {
                    await _repository.DeleteCuisine(cuisine);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.NotFound);
            }
        }

        public async Task<ServerResponse> DeleteClientType(int id)
        {
            ClientType clientType;
            try
            {
                clientType = await _repository.GetClientType(id);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (clientType != null)
            {
                try
                {
                    await _repository.DeleteClientType(clientType);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.NotFound);
            }
        }

        public async Task<ServerResponse> DeleteGoodFor(int id)
        {
            GoodFor foodFor;
            try
            {
                foodFor = await _repository.GetGoodFor(id);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (foodFor != null)
            {
                try
                {
                    await _repository.DeleteGoodFor(foodFor);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.NotFound);
            }
        }

        public async Task<ServerResponse> DeleteSpecialDiet(int id)
        {
            SpecialDiet diet;
            try
            {
                diet = await _repository.GetSpecialDiet(id);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (diet != null)
            {
                try
                {
                    await _repository.DeleteSpecialDiet(diet);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.NotFound);
            }
        }

        public async Task<ServerResponse> DeleteDish(int id)
        {
            Dish dish;
            try
            {
                dish = await _repository.GetDish(id);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (dish != null)
            {
                try
                {
                    await _repository.DeleteDish(dish);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.NotFound);
            }
        }

        public async Task<ServerResponse> DeleteFeature(int id)
        {
            Feature feature;
            try
            {
                feature = await _repository.GetFeature(id);
            }
            catch
            {
                return new ServerResponse(StatusCode.DbConnectionError);
            }
            if (feature != null)
            {
                if (!feature.Editable)
                {
                    return new ServerResponse(StatusCode.Error);
                }
                try
                {
                    await _repository.DeleteFeature(feature);
                    return new ServerResponse(StatusCode.Ok);
                }
                catch
                {
                    return new ServerResponse(StatusCode.Error);
                }
            }
            else
            {
                return new ServerResponse(StatusCode.NotFound);
            }
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
