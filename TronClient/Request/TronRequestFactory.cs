using HDWallet.Core;
using HDWallet.Tron;
using Nethereum.Contracts;
using Nethereum.Contracts.MessageEncodingServices;
using Nethereum.RPC.Eth.DTOs;
using Newtonsoft.Json;
using TronClient.Request.Types;
using TronNet;
using TronNet.Crypto;

namespace TronClient.Request
{
    public class TronRequestFactory : IRequestFactory
    {
        public TriggerConstantContractRequest CreateTriggerConstantContractRequest<TFunctionMessage>(string contractAddress, TronConstantContractFunctionMessage<TFunctionMessage> message) where TFunctionMessage : FunctionMessage
        {
            var contractAddressHex = Base58AddressToHex(contractAddress);
            var callInput = GetFunctionMessageData(contractAddress, message);

            return new TriggerConstantContractRequest
            {
                owner_address = "410000000000000000000000000000000000000000",
                contract_address = contractAddressHex,
                data = callInput.Data,
                visible = message.Visible
            };
        }
        
        public TriggerSmartContractRequest CreateTriggerSmartContractRequest<TFunctionMessage>(IWallet wallet, string contractAddress, TronSmartContractFunctionMessage<TFunctionMessage> message) where TFunctionMessage : FunctionMessage
        {
            var callInput = GetFunctionMessageData(contractAddress, message);
            
            return new TriggerSmartContractRequest
            {
                owner_address = wallet.Address,
                contract_address = contractAddress,
                data = callInput.Data,
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
        
        private static CallInput GetFunctionMessageData<TFunctionMessage>(string contractAddress,
            TronConstantContractFunctionMessage<TFunctionMessage> message) where TFunctionMessage : FunctionMessage
        {
            var functionMessageEncodingService = new FunctionMessageEncodingService<TFunctionMessage>();
            functionMessageEncodingService.SetContractAddress(contractAddress);
            functionMessageEncodingService.DefaultAddressFrom = null;
            return functionMessageEncodingService.CreateCallInput(message.FunctionMessage);
        }
        
        private static string Base58AddressToHex(string contractAddress)
        {
            var contractAddressByte = Base58Encoder.DecodeFromBase58Check(contractAddress);
            return contractAddressByte.ToHex();
        }
    }
}