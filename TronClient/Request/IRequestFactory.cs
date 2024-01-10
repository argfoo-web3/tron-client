using HDWallet.Core;
using HDWallet.Tron;
using Nethereum.Contracts;
using TronClient.Request.Types;

namespace TronClient.Request
{
    public interface IRequestFactory
    {
        TriggerConstantContractRequest CreateTriggerConstantContractRequest<TFunctionMessage>(string contractAddress, TronConstantContractFunctionMessage<TFunctionMessage> message) where TFunctionMessage : FunctionMessage;
        TriggerSmartContractRequest CreateTriggerSmartContractRequest<TFunctionMessage>(IWallet wallet, string contractAddress, TronSmartContractFunctionMessage<TFunctionMessage> message) where TFunctionMessage : FunctionMessage;
        BroadcastRequest CreateBroadcastRequest(Transaction transaction, TronSignature signature);
    }
}