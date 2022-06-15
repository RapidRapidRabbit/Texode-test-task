using System.IO;
using System.Threading.Tasks;
using AnimalsApi.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AnimalsApi.Services
{
    public class ImageService : IImageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly IConfiguration _config;

        public ImageService(IWebHostEnvironment env, IConfiguration config)
        {
            _environment = env;
            _config = config;
        }

        public async Task<string> AddImageReturnPath(IFormFile image)
        {
            string imagesFolder = _config["ImagesFolder"];
            string folderToSave = _environment.WebRootPath + imagesFolder;

            string filename = Path.GetRandomFileName();

            filename = Path.GetFileNameWithoutExtension(filename);
            filename = filename + Path.GetExtension(image.FileName);

            string fullpath = Path.Combine(folderToSave, filename);

            await using (var stream = File.Create(fullpath))
            {
                await image.CopyToAsync(stream);
            }

            string pathtoimg = Path.Combine(imagesFolder, filename);

            return "http://localhost" + pathtoimg;
        }
    }
}
