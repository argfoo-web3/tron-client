namespace TronClient.Request.Types
{
    public class BroadcastRequest
    {
        public string raw_data_hex;
        public string raw_data;
        public string txID;
        public List<string> signature;
        public bool visible;
    }
}