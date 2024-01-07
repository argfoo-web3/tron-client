using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;

namespace TronClient.Http
{
    public class TronHttpClient : IHttpClient
    {
        private readonly HttpClient _httpClient;

        public TronHttpClient(string apiUrl, string apiKey)
        {
            _httpClient = new HttpClient(){ BaseAddress = new Uri(apiUrl)};
            if (!string.IsNullOrEmpty(apiKey))
            {
                _httpClient.DefaultRequestHeaders.Add("TRON_PRO_API_KEY", apiKey);
            }
            _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }
        
        public async Task<T> PostDeserializingResponseAsync<T>(string uri, object? req)
        {
            var requestData = SerializeToJsonContent(req);
            var result = await _httpClient.PostAsync(uri, requestData);
            return await DeserializeResponseAsync<T>(result);
        }

        private static StringContent? SerializeToJsonContent(object? req)
        {
            if (req == null)
            {
                return null;
            }
            var serializedData = JsonConvert.SerializeObject(req);
            return new StringContent(serializedData, Encoding.UTF8, "application/json");
        }
        
        private static async Task<T> DeserializeResponseAsync<T>(HttpResponseMessage responseMessage)
        {
            var response = await responseMessage.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<T>(response);
        }
    }
}