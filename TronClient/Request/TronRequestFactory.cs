using System.Text;
using HDWallet.Core;
using HDWallet.Tron;
using Newtonsoft.Json;
using TronClient.Request.Types;
using TronNet;
using TronNet.ABI;
using TronNet.Crypto;

namespace TronClient.Request
{
    public class TronRequestFactory : IRequestFactory
    {
        public TriggerConstantContractRequest CreateTriggerConstantContractRequest(string contractAddress, TronConstantContractFunctionMessage message)
        {
            var parametersHexString = message.Parameters.ToParametersHexString();
            
            var contractAddressByte = Base58Encoder.DecodeFromBase58Check(contractAddress);
            var contractAddressHex = contractAddressByte.ToHex();
            
            return new TriggerConstantContractRequest
            {
                owner_address = "410000000000000000000000000000000000000000",
                contract_address = contractAddressHex,
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
        internal static string? ToParametersHexString(this IEnumerable<KeyValuePair<string, string>>? parameters)
        {
            if(parameters == null || !parameters.Any())
                return null;
            
            var parametersHexString = new StringBuilder();
            foreach (var param in parameters)
            {
                var abiType = ABIType.CreateABIType(param.Key);

                string paramInHex;
                if (abiType is AddressType)
                {
                    var addressByte = Base58Encoder.DecodeFromBase58Check(param.Value);
                    addressByte = addressByte.Slice(1, addressByte.Length);
                    var addressHex = addressByte.ToHex();
                    paramInHex = abiType.Encode(addressHex).ToHex();
                }
                else
                {
                    paramInHex = abiType.Encode(param.Value).ToHex();
                }
                
                parametersHexString.Append(paramInHex);
            }

            return parametersHexString.ToString();
        }
    }
}