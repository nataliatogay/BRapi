using BR.DTO;
using BR.DTO.Parameters;
using BR.Models;
using BR.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BR.Services.Interfaces
{
    public interface IParameterService
    {
        Task<ServerResponse<ClientParametersInfoResponse>> GetClientParamenters();

        Task<ServerResponse<ICollection<ParameterInfo>>> GetAllMealTypes();

        Task<ServerResponse<ICollection<ParameterInfo>>> GetAllCuisines();

        Task<ServerResponse<ICollection<ParameterInfo>>> GetAllClientTypes();

        Task<ServerResponse<ICollection<ParameterInfo>>> GetAllSpecialDiets();

        Task<ServerResponse<ICollection<ParameterInfo>>> GetAllDishes();

        Task<ServerResponse<ICollection<ParameterInfo>>> GetAllFeatures();

        Task<ServerResponse<ICollection<ParameterInfo>>> GetAllGoodFors();


        Task<ServerResponse<ParameterInfo>> AddCuisine(string title);

        Task<ServerResponse<ParameterInfo>> AddClientType(string title);

        Task<ServerResponse<ParameterInfo>> AddGoodFor(string title);

        Task<ServerResponse<ParameterInfo>> AddFeature(string title);

        Task<ServerResponse<ParameterInfo>> AddDish(string title);

        Task<ServerResponse<ParameterInfo>> AddSpecialDiet(string title);


        Task<ServerResponse<ParameterInfo>> UpdateCuisine(Cuisine cuisine);

        Task<ServerResponse<ParameterInfo>> UpdateClientType(ClientType clientType);

        Task<ServerResponse<ParameterInfo>> UpdateGoodFor(GoodFor goodFor);

        Task<ServerResponse<ParameterInfo>> UpdateSpecialDiet(SpecialDiet diet);

        Task<ServerResponse<ParameterInfo>> UpdateDish(Dish dish);

        Task<ServerResponse<ParameterInfo>> UpdateFeature(Feature feature);


        Task<ServerResponse> DeleteCuisine(int id);

        Task<ServerResponse> DeleteClientType(int id);

        Task<ServerResponse> DeleteGoodFor(int id);

        Task<ServerResponse> DeleteSpecialDiet(int id);

        Task<ServerResponse> DeleteDish(int id);

        Task<ServerResponse> DeleteFeature(int id);




    }
}
