using TronClient.Http;
using TronClient.Request;

namespace TronClient
{
    public class TronClient: ITronClient
    {
        private readonly IHttpClient _httpClient;
        private readonly IRequestFactory _requestFactory;
        
        public TronClient(string apiUrl, string apiKey)
        {
            _httpClient = new TronHttpClient(apiUrl, apiKey);
            _requestFactory = new TronRequestFactory();
        }

        public IContract? GetContract(string contractAddress)
        {
            return string.IsNullOrEmpty(contractAddress) ? null : new TronContract(_httpClient, _requestFactory, contractAddress);
        }
        
        public async Task<BlockExtension> GetNowBlockAsync()
        {
            return await _httpClient.PostDeserializingResponseAsync<BlockExtension>("wallet/getnowblock", null);
        }
    }
}