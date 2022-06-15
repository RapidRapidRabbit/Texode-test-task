using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using AnimalsApi.Models;
using AnimalsApi.Services.Interfaces;

namespace AnimalsApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AnimalsController : ControllerBase
    {
        private readonly IDataService _dataService;

        public AnimalsController(IDataService dataService)
        {
            _dataService = dataService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() => Ok(await _dataService.GetAll());

        [HttpPost]
        public async Task<IActionResult> AddAnimal([FromForm] AnimalRequestModel animal) => Ok(await _dataService.AddAnimal(animal));

        [HttpDelete]
        public async Task<IActionResult> DeleteAnimal([Required] int id)
        {
            var result = await _dataService.DeleteAnimal(id);

            return result ? NoContent() : StatusCode(500);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAnimal([FromForm] AnimalRequestModel animal) => Ok(await _dataService.UpdateAnimal(animal));
    }
}
