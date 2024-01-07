namespace TronClient
{
    public class TronConstantContractFunctionMessage
    {
        public string FunctionSelector { get; set; }
        public KeyValuePair<string, string>[] Parameters { get; set; }
        public bool Visible { get; set; }
    }
    
    public class TronSmartContractFunctionMessage : TronConstantContractFunctionMessage
    {
        public long FeeLimit { get; set; }
        public long CallValue { get; set; }
        public long CallTokenValue { get; set; }
        public long TokenId { get; set; }
    }
}