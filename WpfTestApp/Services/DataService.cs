using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WpfTestApp.Models;

namespace WpfTestApp.Services
{
    public class DataService : IDataService
    {
        private readonly HttpClient _client;
        private const string Api = "http://localhost:80/Animals";

        public DataService()
        {
            //.net core 2.1 or higher :(
            /*var handler = new SocketsHttpHandler
            {
                PooledConnectionLifetime = TimeSpan.FromMinutes(1),
            };
            _client = new HttpClient(handler, disposeHandler: false);*/

            _client = new HttpClient();
        }

        public async Task<Animal> Add(string name, FileInfo file)
        {
            var bytes = File.ReadAllBytes(file.FullName);

            var content = new MultipartFormDataContent();
            content.Add(new StringContent(name), "Name");
            content.Add(new ByteArrayContent(bytes),"File", file.Name );

            var response = await _client.PostAsync($"{Api}/AddAnimal", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            return JsonConvert.DeserializeObject<Animal>(await response.Content.ReadAsStringAsync());
        }

        public async Task<bool> Delete(int id)
        {
            var result = await _client.DeleteAsync($"{Api}/DeleteAnimal?Id={id}");

            return result.IsSuccessStatusCode;
        }

        public async Task<Animal> Update(int id, string name = null, FileInfo file = null)
        {
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(id.ToString()), "Id");

            if (name != null)
            {
                content.Add(new StringContent(name), "Name");
            }

            if (file != null)
            {
                var bytes = File.ReadAllBytes(file.FullName);
                content.Add(new ByteArrayContent(bytes), "File", file.Name);
            }

            var response = await _client.PostAsync($"{Api}/UpdateAnimal", content);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            return JsonConvert.DeserializeObject<Animal>(await response.Content.ReadAsStringAsync());
        }

        public IEnumerable<Animal> GetAll()
        {
            var response = _client.GetAsync($"{Api}/GetAll").Result;

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException();
            }

            var result = JsonConvert.DeserializeObject<List<Animal>>(response.Content.ReadAsStringAsync().Result);

            return result;
        }
    }
}
