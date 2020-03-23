using BR.DTO;
using BR.DTO.Parameters;
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
        Task AddCuisine(ICollection<string> cuisineTitles);
        Task UpdateCuisine(ICollection<Cuisine> cuisine);
        Task<bool> DeleteCuisine(int id);
        Task<ICollection<ParameterInfo>> GetAllClientTypes();
        Task AddClientType(ICollection<string> clientTypeTitles);
        Task UpdateClientType(ICollection<ClientType> clientType);
        Task<bool> DeleteClientType(int id);
    }
}
