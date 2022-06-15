using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using AnimalsApi.Models;
using AnimalsApi.Services.Interfaces;
using Microsoft.Extensions.Configuration;

namespace AnimalsApi.Services
{
    public class DataService : IDataService
    {
        private readonly IImageService _imgService;
        private readonly IConfiguration _config;
        private readonly string _dataDirectory;
        private const string FileExtension = ".json";

        public DataService(IImageService imageService, IConfiguration config)
        {
            _imgService = imageService;
            _config = config;

            _dataDirectory = Path.Combine(Directory.GetCurrentDirectory(), _config["DataFolder"]);
        }

        public async Task<IEnumerable<AnimalDataModel>> GetAll()
        {
            var data = new List<AnimalDataModel>();

            foreach (var fileName in Directory.GetFiles(_dataDirectory, "*.json"))
            {
                using var reader = new StreamReader(Path.Combine(_dataDirectory, fileName));

                string json = await reader.ReadToEndAsync();

                data.Add(JsonSerializer.Deserialize<AnimalDataModel>(json));
            }

            return data;
        }

        public async Task<AnimalDataModel> AddAnimal(AnimalRequestModel animal)
        {
            var id = new Random().Next(0, int.MaxValue);
            var imagePath = await _imgService.AddImageReturnPath(animal.File);

            var animalData = new AnimalDataModel
            {
                Id = id,
                Name = animal.Name,
                Uri = imagePath,
            };

            await using var createStream = File.Create(Path.Combine(_dataDirectory, $"{id}.json"));
            await JsonSerializer.SerializeAsync(createStream, animalData);

            return animalData;
        }

        public async Task<bool> DeleteAnimal(int id)
        {
            var path = Path.Combine(_dataDirectory, id.ToString());

            if (!File.Exists(path + FileExtension))
            {
                return false;
            }

            await Task.Run(() => File.Delete(path + FileExtension));

            return true;
        }

        public async Task<AnimalDataModel> UpdateAnimal(AnimalRequestModel animal)
        {
            var path = Path.Combine(_dataDirectory, animal.Id.ToString());

            if (!File.Exists(path + FileExtension))
            {
                throw new ArgumentException("File Not Found", nameof(animal.Id));
            }

            string json = await File.ReadAllTextAsync(path + FileExtension);
            var data = JsonSerializer.Deserialize<AnimalDataModel>(json);

            if (!string.IsNullOrWhiteSpace(animal.Name))
            {
                data.Name = animal.Name;
            }

            if (animal.File != null)
            {
                data.Uri = await _imgService.AddImageReturnPath(animal.File);
            }

            await using var writer = File.CreateText(path);
            await writer.WriteAsync(JsonSerializer.Serialize(data));

            return data;
        }
    }
}
