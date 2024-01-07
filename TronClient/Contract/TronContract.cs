using HDWallet.Core;
using HDWallet.Tron;
using TronClient.Http;
using TronClient.Request;
using TronNet;

namespace TronClient
{
    public class TronContract : IContract
    {
        private readonly string _contractAddress;
        private readonly IHttpClient _httpClient;
        private readonly IRequestFactory _requestFactory;
        
        internal TronContract(IHttpClient httpClient, IRequestFactory requestFactory, string contractAddress)
        {
            _contractAddress = contractAddress;

            _httpClient = httpClient;
            _requestFactory = requestFactory;
        }

        public async Task<BroadcastResponse> SendContractAsync(IWallet wallet, TronSmartContractFunctionMessage message)
        {
            var transaction = await ExecuteTriggerSmartContractAsync(wallet, message);

            // Signature
            var tronSignature = SignTransaction(wallet, transaction);

            // Broadcast
            return await BroadcastTransactionAsync(transaction, tronSignature);
        }
        
        public async Task<T> CallContractAsync<T>(TronConstantContractFunctionMessage message)
        {
            return await ExecuteTriggerConstantContractAsync<T>(message);
        }

        private async Task<Transaction> ExecuteTriggerSmartContractAsync(IWallet wallet, TronSmartContractFunctionMessage message)
        {
            var transactionRequest = _requestFactory.CreateTriggerSmartContractRequest(wallet, _contractAddress, message);

            var transactionExtension = await SendHttpRequestAsync<TransactionExtension>("wallet/triggersmartcontract", transactionRequest);

            return transactionExtension.transaction;
        }
        
        private async Task<T> ExecuteTriggerConstantContractAsync<T>(TronConstantContractFunctionMessage message)
        {
            var transactionRequest = _requestFactory.CreateTriggerConstantContractRequest(_contractAddress, message);

            return await SendHttpRequestAsync<T>("wallet/triggerconstantcontract", transactionRequest);
        }

        private static TronSignature SignTransaction(IWallet wallet, Transaction transaction)
        {
            var transactionId = transaction.txID.HexToByteArray();
            var signature = wallet.Sign(transactionId);

            return new TronSignature(signature);
        }

        private async Task<BroadcastResponse> BroadcastTransactionAsync(Transaction transaction, TronSignature tronSignature)
        {
            var broadcastReq = _requestFactory.CreateBroadcastRequest(transaction, tronSignature);

            return await SendHttpRequestAsync<BroadcastResponse>("wallet/broadcasttransaction", broadcastReq);
        }
        
        private async Task<T> SendHttpRequestAsync<T>(string path, dynamic request)
        {
            return await _httpClient.PostDeserializingResponseAsync<T>(path, request);
        }
    }
}