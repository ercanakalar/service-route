using System.Net.Http;
using System.Threading.Tasks;
using Backend.Core.Services;

namespace Backend.Service.Services
{
    public class ExternalApiService : IExternalService
    {
        private readonly HttpClient _httpClient;

        public ExternalApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> FetchDataAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                return content;
            }

            throw new HttpRequestException($"Error fetching data: {response.StatusCode}");
        }
    }
}
