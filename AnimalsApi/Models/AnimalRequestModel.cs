using Microsoft.AspNetCore.Http;

namespace AnimalsApi.Models
{
    public class AnimalRequestModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IFormFile File { get; set; }
    }
}
