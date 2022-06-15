using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace AnimalsApi.Services.Interfaces
{
    public interface IImageService
    {
        Task<string> AddImageReturnPath(IFormFile image);
    }
}
