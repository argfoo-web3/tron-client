using HDWallet.Core;
using HDWallet.Tron;
using TronClient.Request.Types;

namespace TronClient.Request
{
    public interface IRequestFactory
    {
        TriggerConstantContractRequest CreateTriggerConstantContractRequest(string contractAddress, TronConstantContractFunctionMessage message);
        TriggerSmartContractRequest CreateTriggerSmartContractRequest(IWallet wallet, string contractAddress, TronSmartContractFunctionMessage message);
        BroadcastRequest CreateBroadcastRequest(Transaction transaction, TronSignature signature);
    }
}