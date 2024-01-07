namespace TronClient.Request.Types
{
    public class TriggerSmartContractRequest : TriggerConstantContractRequest
    {
        public long fee_limit;
        public long call_value;
        public long call_token_value;
        public long token_id;
    }
}