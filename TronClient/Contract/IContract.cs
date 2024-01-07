using HDWallet.Core;

namespace TronClient
{
    public interface IContract
    {
        Task<BroadcastResponse> SendAsync(IWallet wallet, TronSmartContractFunctionMessage message);
        Task<T> CallAsync<T>(TronConstantContractFunctionMessage message);
    }
}