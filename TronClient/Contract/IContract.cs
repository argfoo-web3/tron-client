using HDWallet.Core;

namespace TronClient
{
    public interface IContract
    {
        Task<BroadcastResponse> SendContractAsync(IWallet wallet, TronSmartContractFunctionMessage message);
        Task<T> CallContractAsync<T>(TronConstantContractFunctionMessage message);
    }
}