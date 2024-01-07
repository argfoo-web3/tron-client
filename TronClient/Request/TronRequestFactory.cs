using System.Text;
using HDWallet.Core;
using HDWallet.Tron;
using Newtonsoft.Json;
using TronClient.Request.Types;
using TronNet;
using TronNet.ABI;

namespace TronClient.Request
{
    public class TronRequestFactory : IRequestFactory
    {
        public TriggerConstantContractRequest CreateTriggerConstantContractRequest(string contractAddress, TronConstantContractFunctionMessage message)
        {
            var parametersHexString = message.Parameters.ToParametersHexString();
            
            return new TriggerConstantContractRequest
            {
                contract_address = contractAddress,
                function_selector = message.FunctionSelector,
                parameter = parametersHexString,
                visible = message.Visible
            };
        }

        public TriggerSmartContractRequest CreateTriggerSmartContractRequest(IWallet wallet, string contractAddress, TronSmartContractFunctionMessage message)
        {
            var parametersHexString = message.Parameters.ToParametersHexString();
            
            return new TriggerSmartContractRequest
            {
                owner_address = wallet.Address,
                contract_address = contractAddress,
                function_selector = message.FunctionSelector,
                parameter = parametersHexString,
                fee_limit = message.FeeLimit,
                call_value = message.CallValue,
                call_token_value = message.CallTokenValue,
                token_id = message.TokenId,
                visible = message.Visible
            };
        }

        public BroadcastRequest CreateBroadcastRequest(Transaction transaction, TronSignature signature)
        {
            return new BroadcastRequest
            {
                raw_data_hex = transaction.raw_data_hex,
                raw_data = JsonConvert.SerializeObject(transaction.raw_data),
                txID = transaction.txID,
                signature = new List<string> { signature.SignatureBytes.ToHex() },
                visible = transaction.visible
            };
        }
    }
    
    internal static class ParameterExtension
    {
        internal static string ToParametersHexString(this IEnumerable<KeyValuePair<string, string>> parameters)
        {
            var parametersHexString = new StringBuilder();
            foreach (var param in parameters)
            {
                var abiType = ABIType.CreateABIType(param.Key);
                var paramInHex = abiType.Encode(param.Value).ToHex();
                
                parametersHexString.Append(paramInHex);
            }

            return parametersHexString.ToString();
        }
    }
}