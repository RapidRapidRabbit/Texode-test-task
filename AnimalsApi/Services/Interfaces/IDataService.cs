using System.Collections.Generic;
using System.Threading.Tasks;
using AnimalsApi.Models;

namespace AnimalsApi.Services.Interfaces
{
    public interface IDataService
    {
        Task<IEnumerable<AnimalDataModel>> GetAll();
        Task<AnimalDataModel> AddAnimal(AnimalRequestModel animal);
        Task<bool> DeleteAnimal(int id);
        Task<AnimalDataModel> UpdateAnimal(AnimalRequestModel animal);
    }
}
