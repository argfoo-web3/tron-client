namespace TronClient.Request.Types
{
    public class TriggerSmartContractRequest : TriggerConstantContractRequest
    {
        public string owner_address;
        public long fee_limit;
        public long call_value;
        public long call_token_value;
        public long token_id;
    }
}