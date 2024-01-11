using HDWallet.Core;

namespace TronClient
{
    public interface ITronClient
    {
        IContract? GetContract(string contractAddress);
        Task<BlockExtension> GetNowBlockAsync();
        Task<BlockExtension> GetBlockByNumAsync(long blockNumber);
        Task<Transaction> GetTransactionByIdAsync(string transactionId);
        Task<TransactionInfo> GetTransactionInfoByIdAsync(string transactionId);
    }
}