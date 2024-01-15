using HDWallet.Core;
using Nethereum.Contracts;
using TronNet.ABI.FunctionEncoding.Attributes;

namespace TronClient
{
    public interface IContract
    {
        Task<BroadcastResponse> SendAsync<TFunctionMessage>(IWallet wallet, TronSmartContractFunctionMessage<TFunctionMessage> message) where TFunctionMessage : FunctionMessage;
        Task<TFunctionOutputDto> CallAsync<TFunctionMessage, TFunctionOutputDto>(TronConstantContractFunctionMessage<TFunctionMessage> message) where TFunctionOutputDto : IFunctionOutputDTO, new() where TFunctionMessage : FunctionMessage;
        Task<long> GetEnergyUsed<TFunctionMessage>(TronConstantContractFunctionMessage<TFunctionMessage> message)  where TFunctionMessage : FunctionMessage;
    }
}