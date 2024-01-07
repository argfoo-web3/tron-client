namespace TronClient.Request.Types
{
    public class TriggerConstantContractRequest
    {
        public string owner_address;
        public string contract_address;
        public string function_selector;
        public string? parameter;
        public bool visible;
    }
}