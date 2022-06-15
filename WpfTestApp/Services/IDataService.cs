using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using WpfTestApp.Models;

namespace WpfTestApp.Services
{
    public interface IDataService
    {
        IEnumerable<Animal> GetAll();
        Task<Animal> Add(string name, FileInfo file);
        Task<bool> Delete(int id);
        Task<Animal> Update(int id, string name = null, FileInfo file = null);
    }
}
