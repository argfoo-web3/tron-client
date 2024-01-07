using HDWallet.Core;

namespace TronClient
{
    public interface ITronClient
    {
        IContract GetContract(string contractAddress);
        Task<BlockExtension> GetNowBlockAsync();
        //Task<BroadcastResponse> QuerySmartContractAsync<T>(IWallet wallet, string contractAddress, string functionSelector, string parameter);
        //Task<TransactionResponse> CreateTriggerConstantContractTransactionAsync(string from, string contractAddress, string functionSelector, string parameter, long gasLimit, long callValue = 0, long callTokenValue = 0, long tokenId = 0);
        //Task<TransactionResponse> CreateTriggerSmartContractTransactionAsync(string from, string contractAddress, string functionSelector, string parameter, long gasLimit, long callValue = 0, long callTokenValue = 0, long tokenId = 0);
        //Task<BroadcastResponse> BroadcastTransactionAsync(Transaction transaction);
    }
}