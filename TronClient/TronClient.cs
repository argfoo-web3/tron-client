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
            return await SendHttpRequestAsync<BlockExtension>("wallet/getnowblock");
        }
        
        public async Task<BlockExtension> GetBlockByNumAsync(long blockNumber)
        {
            return await SendHttpRequestAsync<BlockExtension>("wallet/getblockbynum", new { num = blockNumber });
        }
        
        public async Task<Transaction> GetTransactionByIdAsync(string transactionId)
        {
            return await SendHttpRequestAsync<Transaction>("wallet/gettransactionbyid", new { value = transactionId });
        }
        
        public async Task<TransactionInfo> GetTransactionInfoByIdAsync(string transactionId)
        {
            return await SendHttpRequestAsync<TransactionInfo>("wallet/gettransactioninfobyid", new { value = transactionId });
        }

        public async Task<long> GetLatestEnergyPrice()
        {
            var energyPrices = await SendHttpRequestAsync<EnergyPrices>("wallet/getenergyprices");
            var datePriceList = energyPrices.prices.Split(",");

            long lastDate = 0;
            long lastPrice = 0;
            var now = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            foreach (var data in datePriceList)
            {
                var datePrice = data.Split(":");
                if (datePrice.Length != 2 || !long.TryParse(datePrice[0], out var date))
                {
                    continue;
                }
                if (date >= now || date <= lastDate)
                {
                    continue;
                }

                lastDate = date;
                lastPrice = long.Parse(datePrice[1]);
            }

            return lastPrice;
        }

        private async Task<T> SendHttpRequestAsync<T>(string uri, object? req = null)
        {
            return await _httpClient.PostDeserializingResponseAsync<T>(uri, req);
        }
    }
}