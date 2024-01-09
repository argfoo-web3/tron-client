using HDWallet.Core;
using TronNet.ABI.FunctionEncoding.Attributes;

namespace TronClient
{
    public interface IContract
    {
        Task<BroadcastResponse> SendAsync(IWallet wallet, TronSmartContractFunctionMessage message);
        Task<T> CallAsync<T>(TronConstantContractFunctionMessage message)  where T : IFunctionOutputDTO, new();
    }
}