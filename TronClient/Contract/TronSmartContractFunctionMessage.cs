using Nethereum.Contracts;

namespace TronClient
{
    public class TronConstantContractFunctionMessage<TFunctionMessage> where TFunctionMessage : FunctionMessage
    {
        public TFunctionMessage FunctionMessage { get; set; }
        public bool Visible { get; set; }
    }
    
    public class TronSmartContractFunctionMessage<TFunctionMessage> : TronConstantContractFunctionMessage<TFunctionMessage> where TFunctionMessage : FunctionMessage
    {
        public long FeeLimit { get; set; }
        public long CallValue { get; set; }
        public long CallTokenValue { get; set; }
        public long TokenId { get; set; }
    }
}